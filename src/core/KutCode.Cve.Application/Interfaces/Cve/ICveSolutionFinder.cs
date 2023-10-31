
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Models.Solution;

namespace KutCode.Cve.Application.Interfaces.Cve;

public interface ICveSolutionFinder
{
	/// <summary>
	/// Find CVE resolves by report-requested Vulnerability Point
	/// </summary>
	Task<SolutionFinderResult<VulnerabilityPointEntity>> FindAsync(
		ReportRequestVulnerabilityPointDto vulnerabilityPoint,
		IEnumerable<VulnerabilityPointEntity> foundedResolves,
		CancellationToken ct = default);
}