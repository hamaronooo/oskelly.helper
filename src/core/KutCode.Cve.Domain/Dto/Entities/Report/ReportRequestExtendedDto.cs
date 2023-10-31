namespace KutCode.Cve.Domain.Dto.Entities.Report;

public record ReportRequestExtendedDto : ReportRequestDto
{
	public List<ReportRequestVulnerabilityPointDto> Vulnerabilities { get; init; } = new();
}