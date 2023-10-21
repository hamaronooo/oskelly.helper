using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutCode.Cve.Domain.Entities;

[Description("Common Vulnerabilities and Exposures")]
[Table("cve")]
public sealed class CveEntity 
{
	public CveEntity() {}

	public CveEntity(CveId id)
	{
		Year = id.Year;
		CnaNumber = id.CnaNumber;
	}

	public CveEntity(int year, string cnaNumber)
	{
		Year = year;
		CnaNumber = cnaNumber;
	}

	[Description("CVE-XXXX representation. Primary key.")]
	[Column("year")]
	public int Year { get; set; }
	
	[Description(" CVE-...-XXXX representation. Primary key.")]
	[Column("cna_number"), MaxLength(20)]
	public string CnaNumber { get; set; }

	[NotMapped] public CveId CveId => new(Year, CnaNumber);

	[Description("CVE short name representation")]
	[Column("short_name")]
	public string? ShortName { get; set; }
	
	[Description("English description of CVE properties and effects")]
	[Column("description_en")]
	public string? DescriptionEnglish { get; init; }
	
	[Description("Russian description of CVE properties and effects")]
	[Column("description_ru")]
	public string? DescriptionRussian { get; init; }

	[Description("Common Vulnerability Scoring System")]
	[Column("cvss")]
	public double? CVSS { get; init; }
	
	[Description("Locked with update (or something else) process")]
	[Column("locked")]
	public bool Locked { get; set; }

	public ICollection<VulnerabilityPointEntity> Vulnerabilities { get; set; } = new List<VulnerabilityPointEntity>();

	public override string ToString() => CveId.ToString();
}