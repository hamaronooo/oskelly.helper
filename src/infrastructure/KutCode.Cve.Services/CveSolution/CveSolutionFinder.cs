using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Models.Solution;
using Lifti;
using Lifti.Querying;
using Lifti.Querying.QueryParts;
using Lifti.Tokenization;

namespace KutCode.Cve.Services.CveSolution;

/*
 * Ищем полнотекстовым поиском в найденных решениях CVE.
 * Если у нас есть что-то одно, платформа или софт - оринтируемся, естественно, на то, что есть.
 * Если у нас есть И платформа И софт вместе - важнее для нас будет именно софт, а уже затем платформа.
*/
public sealed class CveSolutionFinder : ICveSolutionFinder
{
	public async Task<SolutionFinderResult<VulnerabilityPointEntity>> FindAsync(
		ReportRequestVulnerabilityPointDto vulnerabilityPoint, 
		IEnumerable<VulnerabilityPointEntity> foundedResolves,
		CancellationToken ct = default)
	{
		if (string.IsNullOrEmpty(vulnerabilityPoint.Software) && string.IsNullOrEmpty(vulnerabilityPoint.Platform))
			return new();
		var resolvesList = foundedResolves?.ToList();
		if (resolvesList is null || resolvesList.Count() == 0)
			return new();

		List<(Guid Id, double Score)> results = new();
		if (string.IsNullOrEmpty(vulnerabilityPoint.Software) is false)
		{
			FullTextIndex<Guid> index = await GetSoftwareIndex(resolvesList, ct);
			Query query = GetQuery(vulnerabilityPoint.Software, index.DefaultTokenizer);
			ISearchResults<Guid> a = index.Search(query.ToString()!);
			results.AddRange(a.Select(x => (x.Key, x.Score)));
		}
		if (string.IsNullOrEmpty(vulnerabilityPoint.Platform) is false)
		{
			FullTextIndex<Guid> index = await GetPlatformIndex(resolvesList, ct);
			Query query = GetQuery(vulnerabilityPoint.Platform, index.DefaultTokenizer);
			ISearchResults<Guid> a = index.Search(query.ToString()!);
			results.AddRange(a.Select(x => (x.Key, x.Score)));
		}

		var result = results.GroupBy(x => x.Id)
			.Select(x => new { x.Key, Total = x.Count() * x.Sum(s => s.Score) })
			.Join(resolvesList, arg => arg.Key, entity => entity.Id,
				(searchResult, entity) =>
					new SolutionFinderResultItem<VulnerabilityPointEntity>(entity, searchResult.Total));
		return new(result);
	}

	private Query GetQuery(string prompt, IIndexTokenizer tokenizer)
	{
		var promptParts = prompt.Split(' ', '_');
		Query query;
		if (promptParts.Length == 1) {
			// ввести дистанцию и edits зависимыми от длины строки
			ushort distance = (ushort)(prompt.Length <= 15 ? 2 : 3);
			ushort edits = (ushort)(prompt.Length <= 15 ? 2 : 4);
			query = new Query(new FuzzyMatchQueryPart(tokenizer.Normalize(prompt), distance, edits));
		}
		else if (promptParts.Length == 2)
			query = new Query(
				new AndQueryOperator(new FuzzyMatchQueryPart(tokenizer.Normalize(promptParts[0]), 2), 
					new FuzzyMatchQueryPart(tokenizer.Normalize(promptParts[1]), 4, 2)));
		else {
			var firstPart = new AndQueryOperator(new FuzzyMatchQueryPart(tokenizer.Normalize(promptParts[0]), 2),
				new FuzzyMatchQueryPart(tokenizer.Normalize(promptParts[1]), 4, 2));
			var secondPart = new FuzzyMatchQueryPart(string.Join(' ', promptParts[2..].Select(x => tokenizer.Normalize(x))), 6, 3);
			query = new Query(
				new AndQueryOperator(firstPart, secondPart));
		}
		// todo: если первый поиск не дал результатов - повторить с более гибкой query 
		// todo: если part содержит более 1 точки, не применять минимальный Fuzzy 
		return query;
	}

	async Task<FullTextIndex<Guid>> GetPlatformIndex(IEnumerable<VulnerabilityPointEntity> foundedResolves, CancellationToken ct)
	{
		FullTextIndex<Guid> index = new FullTextIndexBuilder<Guid>().Build();
		foreach (var resolve in foundedResolves)
			if (string.IsNullOrEmpty(resolve.Platform?.Name) == false)
				await index.AddAsync(resolve.Id, resolve.Platform?.Name!, ct);
		return index;
	}
	async Task<FullTextIndex<Guid>> GetSoftwareIndex(IEnumerable<VulnerabilityPointEntity> foundedResolves, CancellationToken ct)
	{
		FullTextIndex<Guid> index = new FullTextIndexBuilder<Guid>().Build();
		foreach (var resolve in foundedResolves)
			if (string.IsNullOrEmpty(resolve.Software?.Name) == false)
				await index.AddAsync(resolve.Id, resolve.Software?.Name!, ct);
		return index;
	}
}