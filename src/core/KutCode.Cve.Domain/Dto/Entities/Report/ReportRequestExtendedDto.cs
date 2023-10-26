namespace KutCode.Cve.Domain.Dto.Entities.Report;

public class ReportRequestExtendedDto : ReportRequestDto
{
	public List<ReportRequestVulnerabilityPointDto> Cve { get; init; } = new();
}