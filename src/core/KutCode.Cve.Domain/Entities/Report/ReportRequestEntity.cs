using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using KutCode.Cve.Domain.Entities.Base;
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Entities.Report;

[Table("report_request")]
public sealed class ReportRequestEntity : ModelWithId<Guid>
{
	[Description("User-defined name just for user")]
	[Column("custom_name")]
	public string? CustomName { get; set; }

	[Column("state")]
	public ReportRequestState State { get; set; } = ReportRequestState.Created;

	[Column("search_strategy")]
	public ReportSearchStrategy SearchStrategy { get; set; } = ReportSearchStrategy.Combine;

	/// <summary>
	/// Resolver Code через разделитель ';'
	/// </summary>
	[Column("sources")]
	public string SourcesRaw { get; set; }
	[NotMapped]
	public string[] Sources => SourcesRaw.Split(';');
	public void SetSources(IEnumerable<string> sources) => SourcesRaw = string.Join(';', sources.Select(x => x.Trim().ToLower()).Distinct());

	public ICollection<ReportRequestCveEntity> Cve { get; set; } = new List<ReportRequestCveEntity>();
}