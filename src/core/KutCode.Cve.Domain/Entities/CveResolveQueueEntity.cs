using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutCode.Cve.Domain.Entities;

[Table("cve_resolve_queue")]
public sealed class CveResolveQueueEntity
{
	public CveResolveQueueEntity() { }

	public CveResolveQueueEntity(CveId cveId, string resolverCode, int priority = 0)
	{
		CveYear = cveId.Year;
		CveCnaNumber = cveId.CnaNumber;
		ResolverCode = resolverCode;
		Priority = priority;
	}
	public CveResolveQueueEntity(int cveYear, string cveCnaNumber, string resolverCode, int priority = 0)
	{
		CveYear = cveYear;
		CveCnaNumber = cveCnaNumber;
		ResolverCode = resolverCode;
		Priority = priority;
	}
	
	[Column("cve_year")]
	public int CveYear { get; set; }
	[Column("cve_cna_number")]
	public string CveCnaNumber { get; set; }

	[NotMapped]
	public CveId CveId => new(CveYear, CveCnaNumber); 

	[Description("Should update CVE properties")]
	[Column("update_cve")]
	public bool UpdateCve { get; set; }
	
	[Column("resolver_code")]
	public string ResolverCode { get; set; }

	[Column("priority")] 
	public int Priority { get; set; } = 0;
	
	[Column("sys_created", TypeName = "timestamp without time zone")]
	public DateTime SysCreated { get; set; } = DateTime.Now;
}