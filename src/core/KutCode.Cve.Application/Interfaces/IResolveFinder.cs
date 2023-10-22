namespace KutCode.Cve.Application.Interfaces;

public interface IResolveFinder
{
	public string FinderCode { get; }
	Task<IEnumerable<VulnerabilityPointEntity>> FindAsync(CveId cveId, CancellationToken ct = default);
}