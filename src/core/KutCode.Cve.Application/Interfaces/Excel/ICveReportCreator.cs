using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Models.Solution;

namespace KutCode.Cve.Application.Interfaces.Excel;

public interface ICveReportCreator
{
	/// <summary>
	/// Create Excel report by provided report-request and solutions 
	/// </summary>
	Task<byte[]> CreateExcelReportAsync(
		ReportRequestExtendedDto reportRequest,
		IEnumerable<SolutionFinderResult<VulnerabilityPointEntity>> vulnerabilities,
		CancellationToken ct = default);
}