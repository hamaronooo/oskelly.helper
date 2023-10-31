
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Dto.Entities.Report;

public class ReportRequestDto
{
	public Guid Id { get; init; }
	public string? CustomName { get;init; }
	
	public ReportRequestState State { get;init; }
	public string StateName => EnumHelper.GetDescriptionValue(State);
	public ReportSearchStrategy SearchStrategy { get;init; }
	public string SearchStrategyName => EnumHelper.GetDescriptionValue(SearchStrategy);
	public DateTime? SysCreated { get;init; }
	/// <summary>
	/// Resolver Code через разделитель ';'
	/// </summary>
	public string SourcesRaw { get;init; }
	public string[] Sources => SourcesRaw.Split(';');
}