using System.ComponentModel.DataAnnotations.Schema;

namespace KutCode.Cve.Domain.Entities;

[Table("cve_finder_queue")]
public sealed class CveFinderQueueEntity
{
	public CveFinderQueueEntity() { }

	public CveFinderQueueEntity(CveId cveId, string finderCode, int priority = 0)
	{
		CveYear = cveId.Year;
		CveCnaNumber = cveId.CnaNumber;
		FinderCode = finderCode;
		Priority = priority;
	}
	public CveFinderQueueEntity(int cveYear, string cveCnaNumber, string finderCode, int priority = 0)
	{
		CveYear = cveYear;
		CveCnaNumber = cveCnaNumber;
		FinderCode = finderCode;
		Priority = priority;
	}
	
	[Column("cve_year")]
	public int CveYear { get; set; }
	[Column("cve_cna_number")]
	public string CveCnaNumber { get; set; }

	[NotMapped]
	public CveId CveId => new CveId(CveYear, CveCnaNumber); 

	[Column("finder_code")]
	public string FinderCode { get; set; }

	[Column("priority")] 
	public int Priority { get; set; } = 0;
	
	[Column("sys_created", TypeName = "timestamp without time zone")]
	public DateTime SysCreated { get; set; } = DateTime.Now;
}