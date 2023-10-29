using System.Text;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Helpers;
using KutCode.Cve.Domain.Models.Solution;
using Lifti;
using Lifti.Tokenization;

namespace KutCode.Cve.Services.CveSolution;

/*
 * Ищем полнотекстовым поиском в найденных решениях CVE.
 * Если у нас есть что-то одно, платформа или софт - оринтируемся, естественно, на то, что есть.
 * Если у нас есть И платформа И софт вместе - важнее для нас будет именно софт, а уже затем платформа.
*/
public sealed class CveSolutionFinderTest : ICveSolutionFinder
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

		FullTextIndex<Guid> platformIndex = await GetPlatformIndex(resolvesList, ct);
		FullTextIndex<Guid> softwareIndex = await GetSoftwareIndex(resolvesList, ct);
		List<(Guid Id, double Score)> results = new();
		var softwareQueryDto = GetQuery(NamesNormalizer.NormalizeSoftwareName(vulnerabilityPoint.Software), platformIndex.DefaultTokenizer);
		var platformQueryDto = GetQuery(NamesNormalizer.NormalizeSoftwareName(vulnerabilityPoint.Platform), platformIndex.DefaultTokenizer);
		if (softwareQueryDto.IsPromptValid)
		{
			ISearchResults<Guid> platformResult = platformIndex.Search(softwareQueryDto.Query);
			results.AddRange(platformResult.Select(x => (x.Key, x.Score)));
			ISearchResults<Guid> softwareResult = softwareIndex.Search(softwareQueryDto.Query);
			results.AddRange(softwareResult.Select(x => (x.Key, x.Score)));
		}
		if (platformQueryDto.IsPromptValid)
		{
			ISearchResults<Guid> platformResult = platformIndex.Search(platformQueryDto.Query);
			results.AddRange(platformResult.Select(x => (x.Key, x.Score)));
			ISearchResults<Guid> softwareResult = softwareIndex.Search(platformQueryDto.Query);
			results.AddRange(softwareResult.Select(x => (x.Key, x.Score)));
		}

		var result = results.GroupBy(x => x.Id)
			.Select(x => new { x.Key, Total = x.Count() * x.Sum(s => s.Score) })
			.Join(resolvesList, arg => arg.Key, entity => entity.Id,
				(searchResult, entity) =>
					new SolutionFinderResultItem<VulnerabilityPointEntity>(entity, searchResult.Total));
		return new(result);
	}

	private (string Query, bool IsPromptValid) GetQuery(string prompt, IIndexTokenizer tokenizer)
	{
		if (string.IsNullOrWhiteSpace(prompt)) return (null, false)!;
		var promptParts = prompt.Split(' ', '_')
			.Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();

		var str = new StringBuilder();
		for (int i = 0; i < promptParts.Length; i++)
		{
			str.Append(promptParts[i].Trim());
			if (i != promptParts.Length - 1)
				if (!string.IsNullOrEmpty(promptParts[i]))
					str.Append(" | ");
		}

		return (str.ToString(), true);
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