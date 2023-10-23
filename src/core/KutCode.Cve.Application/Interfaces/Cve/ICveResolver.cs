namespace KutCode.Cve.Application.Interfaces.Cve;

/// <summary>
/// Service, that finds CVE vulnerabilities and resolves.
/// In addition, may found some CVE description updates.
/// </summary>
public interface ICveResolver
{
	public string Code { get; }
	Task<IEnumerable<VulnerabilityPointEntity>> FindAsync(CveId cveId, CancellationToken ct = default);
}