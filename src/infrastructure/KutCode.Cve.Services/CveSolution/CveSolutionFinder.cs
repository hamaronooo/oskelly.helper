using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Models.Solution;
using Lifti;

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
			// todo: build and use query for search // always convert to string
			ISearchResults<Guid> a = index.Search("");
			results.AddRange(a.Select(x => (x.Key, x.Score)));
		}
		if (string.IsNullOrEmpty(vulnerabilityPoint.Platform) is false)
		{
			FullTextIndex<Guid> index = await GetPlatformIndex(resolvesList, ct);
			// todo: build and use query for search // always convert to string
			ISearchResults<Guid> a = index.Search("");
			results.AddRange(a.Select(x => (x.Key, x.Score)));
		}

		var result = results.GroupBy(x => x.Id)
			.Select(x => new { x.Key, Total = x.Count() * x.Sum(s => s.Score) })
			.Join(resolvesList, arg => arg.Key, entity => entity.Id,
				(searchResult, entity) =>
					new SolutionFinderResultItem<VulnerabilityPointEntity>(entity, searchResult.Total));
		return new(result);
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