namespace KutCode.Cve.Domain.Dto.Entities.Report;

public class ReportRequestExtendedDto : ReportRequestDto
{
	public List<ReportRequestCveDto> Cve { get; init; } = new();
}