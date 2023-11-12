
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Dto.Entities.Report;

public record ReportRequestDto
{
	public Guid Id { get; init; }
	public string? CustomName { get;init; }
	
	public ReportRequestState State { get;init; }
	public string StateName => EnumHelper.GetDescriptionValue(State);
	public ReportSearchStrategy SearchStrategy { get;init; }
	public string SearchStrategyName => EnumHelper.GetDescriptionValue(SearchStrategy);
	public DateTime? SysCreated { get;init; }
	/// <summary>
	/// Сортировать вывод по CVE ID
	/// </summary>
	public bool IsReorder { get; set; } = false;
	/// <summary>
	/// Resolver Code через разделитель ','
	/// </summary>
	public string SourcesRaw { get;init; }
	public string[] Sources => SourcesRaw.Split(',');
}