using KutCode.Cve.Domain.Dto.Entities.Report;

namespace KutCode.Cve.Application.Interfaces.Excel;

public interface IReportRequestParser
{
	List<ReportRequestCveDto> ParseXlsxReportRequestCve(Stream fileStream);
	//List<ReportRequestCveDto> ParseCsvReportRequestCve(Stream fileStream);
}