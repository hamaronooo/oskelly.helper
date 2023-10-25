
namespace KutCode.Cve.Domain.Dto.Entities.Report;

public sealed record ReportRequestCveDto
{
	public Guid Id { get; init; }
	public int CveYear { get; init; }
	public string CveCnaNumber { get; init; }
	public string CveId => new CveId(CveYear, CveCnaNumber).ToString();
	
	public string? Platform { get; init; }
	public string? Software { get; init; }
	public string? CveDescription { get; init; }
	
	public Guid ReportRequestId { get; init; }
}