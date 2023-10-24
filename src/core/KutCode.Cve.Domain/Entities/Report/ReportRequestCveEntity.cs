
using System.ComponentModel.DataAnnotations.Schema;
using KutCode.Cve.Domain.Entities.Base;

namespace KutCode.Cve.Domain.Entities.Report;

[Table("report_request_cve")]
public sealed class ReportRequestCveEntity : ModelWithId<Guid>
{
	public ReportRequestCveEntity() { }
	public ReportRequestCveEntity(CveId cveId, string platform, string software)
	{
		CveYear = cveId.Year;
		CveCnaNumber = cveId.CnaNumber;
		Platform = platform;
		Software = software;
	}

	[Column("cve_year")]
	public int CveYear { get; set; }
	[Column("cve_cna_number")]
	public string CveCnaNumber { get; set; }

	[Column("platform")]
	public string? Platform { get; set; }
	[Column("software")]
	public string? Software { get; set; }

	[Column("report_request_id")]
	public Guid ReportRequestId { get; set; }
	public ReportRequestEntity ReportRequest { get; set; }
}