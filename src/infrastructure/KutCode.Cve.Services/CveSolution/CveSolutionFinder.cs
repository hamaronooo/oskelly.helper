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
public sealed class CveSolutionFinder : ICveSolutionFinder
{
	public async Task<SolutionFinderResult<VulnerabilityPointEntity>> FindAsync(
		ReportRequestVulnerabilityPointDto vulnerabilityPoint,
		IEnumerable<VulnerabilityPointEntity> foundedResolves,
		CveSolutionFinderSettings? settings = null,
		CancellationToken ct = default)
	{
		settings ??= new();
		if (string.IsNullOrEmpty(vulnerabilityPoint.Software) && string.IsNullOrEmpty(vulnerabilityPoint.Platform))
		{
			if (settings.ShowResultsIfEmptyPrompt is false)
				return new SolutionFinderResult<VulnerabilityPointEntity>();
			else return GetDefault(vulnerabilityPoint, foundedResolves);
		}
		
		
		var resolvesList = foundedResolves?.ToList();
		if (resolvesList is null || resolvesList.Count() == 0)
			return new SolutionFinderResult<VulnerabilityPointEntity>();
 
		var platformIndex = await GetPlatformIndex(resolvesList, ct);
		var softwareIndex = await GetSoftwareIndex(resolvesList, ct);
		List<(Guid Id, double Score)> results = new();
		var softwareQueryDto = GetQuery(NamesNormalizer.NormalizeSoftwareName(vulnerabilityPoint.Software),
			platformIndex.DefaultTokenizer);
		var platformQueryDto = GetQuery(NamesNormalizer.NormalizeSoftwareName(vulnerabilityPoint.Platform),
			platformIndex.DefaultTokenizer);

		if (softwareQueryDto.IsPromptValid)
		{
			var platformResult = platformIndex.Search(softwareQueryDto.Query);
			results.AddRange(platformResult.Select(x => (x.Key, x.Score)));
			var softwareResult = softwareIndex.Search(softwareQueryDto.Query);
			results.AddRange(softwareResult.Select(x => (x.Key, x.Score)));
		}
		if (platformQueryDto.IsPromptValid)
		{
			var platformResult = platformIndex.Search(platformQueryDto.Query);
			results.AddRange(platformResult.Select(x => (x.Key, x.Score)));
			var softwareResult = softwareIndex.Search(platformQueryDto.Query);
			results.AddRange(softwareResult.Select(x => (x.Key, x.Score)));
		}

		var result = results
			.GroupBy(x => x.Id)
			.Select(x => new { x.Key, Total = x.Count() * x.Sum(s => s.Score) })
			.Join(resolvesList, arg => arg.Key, entity => entity.Id,
				(searchResult, entity) => {
					return new SolutionFinderResultItem<VulnerabilityPointEntity>(vulnerabilityPoint.CveId, entity, searchResult.Total);
				});
		return new SolutionFinderResult<VulnerabilityPointEntity>(result);
	}

	private SolutionFinderResult<VulnerabilityPointEntity> GetDefault(ReportRequestVulnerabilityPointDto vulnerabilityPoint, IEnumerable<VulnerabilityPointEntity> foundedResolves)
	{
		return new SolutionFinderResult<VulnerabilityPointEntity>
		{
			Resolves = foundedResolves.Select(x =>
				new SolutionFinderResultItem<VulnerabilityPointEntity>(vulnerabilityPoint.CveId, x, 0)).ToList()
		};
	}

	private (string Query, bool IsPromptValid) GetQuery(string prompt, IIndexTokenizer tokenizer)
	{
		if (string.IsNullOrWhiteSpace(prompt)) return (null, false)!;
		var promptParts = prompt.Split(' ', '_')
			.Where(x => x.Length > 1)
			.Where(x => string.IsNullOrWhiteSpace(x) == false && string.IsNullOrEmpty(x) == false)
			.Select(x => x.Normalize().Trim())
			.ToArray();

		var str = new StringBuilder();
		for (var i = 0; i < promptParts.Length; i++)
		{
			if (string.IsNullOrWhiteSpace(promptParts[i]) || string.IsNullOrEmpty(promptParts[i])) continue;
			if (i != 0)
				str.Append(" | ");
			str.Append('?');
			str.Append(promptParts[i].Trim());
		}

		return (str.ToString(), true);
	}

	private async Task<FullTextIndex<Guid>> GetPlatformIndex(IEnumerable<VulnerabilityPointEntity> foundedResolves,
		CancellationToken ct)
	{
		var index = new FullTextIndexBuilder<Guid>().Build();
		foreach (var resolve in foundedResolves)
			if (string.IsNullOrEmpty(resolve.Platform?.Name) == false)
				await index.AddAsync(resolve.Id, resolve.Platform?.Name!, ct);
		return index;
	}

	private async Task<FullTextIndex<Guid>> GetSoftwareIndex(IEnumerable<VulnerabilityPointEntity> foundedResolves,
		CancellationToken ct)
	{
		var index = new FullTextIndexBuilder<Guid>().Build();
		foreach (var resolve in foundedResolves)
			if (string.IsNullOrEmpty(resolve.Software?.Name) == false)
				await index.AddAsync(resolve.Id, resolve.Software?.Name!, ct);
		return index;
	}
}