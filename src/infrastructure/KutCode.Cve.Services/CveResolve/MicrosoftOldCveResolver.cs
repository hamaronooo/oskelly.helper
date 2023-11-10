using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Enums;
using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.ApiRepositories.Microsoft.Models;
using KutCode.Cve.Services.ApiRepositories.Mitre;

namespace KutCode.Cve.Services.CveResolve;

/// <summary>
/// Resolve CVE from MSRC site from old Microsoft vulnerability system (non CVE)
/// </summary>
[CveResolver("msrc_old", "Устаревший репозиторий Microsoft (до 2018)", "learn.microsoft.com", false)]
public sealed class MicrosoftOldCveResolver : ICveResolver
{
	private readonly MicrosoftSecurityApiRepository _msrcApi;
	private readonly MitreApiRepository _mitreApi;

	public MicrosoftOldCveResolver(
		MicrosoftSecurityApiRepository msrcApi,
		MitreApiRepository mitreApi)
	{
		_msrcApi = msrcApi;
		_mitreApi = mitreApi;
	}

	public string Code => "msrc_old";
	public Uri Uri => new("https://learn.microsoft.com/");

	// todo: finish her
	public async Task<IEnumerable<VulnerabilityPointEntity>> ResolveAsync(CveId cveId, CancellationToken ct = default)
	{
		var mitreCve = await _mitreApi.GetCveAsync(cveId, ct);
		if (mitreCve.Data == null || mitreCve.IsSuccessful == false) return Enumerable.Empty<VulnerabilityPointEntity>();
		foreach (var reference in mitreCve.Data.Containers.Cna.References) {
			Uri refUri = new(reference.Url);
			if (refUri.Host == "learn.microsoft.com" || refUri.Host == "docs.microsoft.com") {
				// gototo link, download xml page and tryparse it
				// try get Affected Software block table
				
			}
		}
		return Enumerable.Empty<VulnerabilityPointEntity>();
	}
}