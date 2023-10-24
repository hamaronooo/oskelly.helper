
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Dto.Entities.Report;

public sealed class ReportRequestDto
{
	public Guid Id { get; init; }
	public string? CustomName { get; set; }
	
	public ReportRequestState State { get; set; }
	public ReportSearchStrategy SearchStrategy { get; set; }

	/// <summary>
	/// Resolver Code через разделитель ';'
	/// </summary>
	public string SourcesRaw { get; set; }
	public string[] Sources => SourcesRaw.Split(';');

	public List<ReportRequestCveDto> Cve { get; set; }
}