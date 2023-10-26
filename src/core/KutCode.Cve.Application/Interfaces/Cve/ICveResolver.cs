namespace KutCode.Cve.Application.Interfaces.Cve;

/// <summary>
/// Service, that finds CVE vulnerabilities and resolves.
/// In addition, may found some CVE description updates.
/// </summary>
public interface ICveResolver
{
	public string Code { get; }
	public Uri Uri { get; } // make compare by .Host
	Task<IEnumerable<VulnerabilityPointEntity>> ResolveAsync(CveId cveId, CancellationToken ct = default);
}