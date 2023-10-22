using System.ComponentModel.DataAnnotations.Schema;

namespace KutCode.Cve.Domain.Entities;

public sealed class CveFinderQueueEntity
{
	[Column("cve_year")]
	public int CveYear { get; set; }
	[Column("cve_cna_number")]
	public string CveCnaNumber { get; set; }
	public CveEntity Cve { get; set; }

	[Column("finder_code")]
	public string FinderCode { get; set; }

	[Column("priority")] 
	public int Priority { get; set; } = 0;
	
	[Column("sys_created", TypeName = "datetime without timezone")]
	public DateTime SysCreated { get; set; } = DateTime.Now;
}