
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Dto.Entities.Report;

public sealed class ReportRequestDto
{
	public Guid Id { get; init; }
	public string? CustomName { get;init; }
	
	public ReportRequestState State { get;init; }
	public ReportSearchStrategy SearchStrategy { get;init; }
	public DateTime SysCreated { get;init; }
	/// <summary>
	/// Resolver Code через разделитель ';'
	/// </summary>
	public string SourcesRaw { get;init; }
	public string[] Sources => SourcesRaw.Split(';');
}