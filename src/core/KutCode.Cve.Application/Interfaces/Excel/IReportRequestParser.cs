using KutCode.Cve.Domain.Dto.Entities.Report;

namespace KutCode.Cve.Application.Interfaces.Excel;

public interface IReportRequestParser
{
	List<ReportRequestVulnerabilityPointDto> ParseXlsxReportRequestCve(Stream fileStream);
	//List<ReportRequestVulnerabilityPointDto> ParseCsvReportRequestCve(Stream fileStream);
}