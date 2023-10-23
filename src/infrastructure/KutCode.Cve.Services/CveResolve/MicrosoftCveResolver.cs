using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Enums;
using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.ApiRepositories.Microsoft.Models;

namespace KutCode.Cve.Services.CveResolve;

public sealed class MicrosoftCveResolver : ICveResolver
{
	private readonly MicrosoftSecurityApiRepository _msrcApi;

	public MicrosoftCveResolver(MicrosoftSecurityApiRepository msrcApi)
	{
		_msrcApi = msrcApi;
	}

	public string Code => "msrc";
	
	public async Task<IEnumerable<VulnerabilityPointEntity>> FindAsync(CveId cveId, CancellationToken ct = default)
	{
		var msResponse = await _msrcApi.GetCveDataAsync(cveId, ct);
		if (msResponse.IsSuccessful is false)
			throw new HttpRequestException(msResponse.Content, null, msResponse.StatusCode);

		var result = new List<VulnerabilityPointEntity>(msResponse.Data?.Value.Count ?? 0);
		foreach (var item in msResponse.Data?.Value ?? Enumerable.Empty<MicrosoftKbValueItem>())
		{
			if (CveId.Parse(item.CveNumber) != cveId) continue;
			var solutions = GetSolutions(item);
			if (solutions.Count == 0) continue;
			var platform = GetPlatform(item);
			var software = GetSoftware(item);
			result.Add(new() {
				CveYear = cveId.Year,
				CveCnaNumber = cveId.CnaNumber,
				DataSourceCode = Code,
				Impact = item.Impact,
				Platform = platform,
				Software = software?.Name == platform?.Name ? null : software,
				CveSolutions = solutions
			});
		}

		return result;
	}

	private List<CveSolutionEntity> GetSolutions(MicrosoftKbValueItem item)
	{
		if (item.KbArticles.Count == 0) return Enumerable.Empty<CveSolutionEntity>().ToList();
		var result = new List<CveSolutionEntity>(item.KbArticles.Count);
		foreach (var article in item.KbArticles)
		{
			result.Add(new ()
			{
				Info = $"KB{article.ArticleName}",
				Description = string.IsNullOrEmpty(article.FixedBuildNumber)
					? article.DownloadName :  $"Исправлено в сбоке {article.FixedBuildNumber}",
				DownloadLink = article.DownloadUrl,
				SolutionLink = article.ArticleUrl,
				AdditionalLink = article.KnownIssuesUrl
			});
		}
		return result;
	}

	private PlatformEntity? GetPlatform(MicrosoftKbValueItem item)
	{
		var platform = new PlatformEntity();

		if (string.IsNullOrEmpty(item.Platform))
			return FindPlatformInProduct(item);
		
		if (item.Platform.ToLower().Contains("windows")) {
			platform.PlatformType = PlatformType.Windows;
		}
		else if (item.Platform.ToLower().Contains("linux")
		         || item.Platform.ToLower().Contains("ubuntu")
		         || item.Platform.ToLower().Contains("debian")
		         || item.Platform.ToLower().Contains("oracle")
		         || item.Platform.ToLower().Contains("centos"))
		{
			platform.PlatformType = PlatformType.Linux;
		}
		else if (item.Platform.ToLower().Contains("mac os")
		         || item.Platform.ToLower().Contains("ios")
		         || item.Platform.ToLower().Contains("macos"))
		{
			platform.PlatformType = PlatformType.Apple;
		}
		platform.Name = item.Platform.Trim();
		return platform;
	}

	private PlatformEntity? FindPlatformInProduct(MicrosoftKbValueItem item)
	{
		var platform = new PlatformEntity();
		
		if (item.Product.ToLower().Contains("windows")) {
			platform.PlatformType = PlatformType.Windows;
		}
		else if (item.Product.ToLower().Contains("linux")
		         || item.Product.ToLower().Contains("ubuntu")
		         || item.Product.ToLower().Contains("debian")
		         || item.Product.ToLower().Contains("oracle")
		         || item.Product.ToLower().Contains("centos"))
		{
			platform.PlatformType = PlatformType.Linux;
		}
		else if (item.Product.ToLower().Contains("mac os")
		         || item.Product.ToLower().Contains("ios")
		         || item.Product.ToLower().Contains("macos"))
		{
			platform.PlatformType = PlatformType.Apple;
		}
		platform.Name = item.Product.Trim();
		return platform;
	}

	private SoftwareEntity? GetSoftware(MicrosoftKbValueItem item)
	{
		return new SoftwareEntity {Name = item.Product.Trim()};
	}
}