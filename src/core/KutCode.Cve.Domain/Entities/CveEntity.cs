using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KutCode.Cve.Domain.Models;

namespace KutCode.Cve.Domain.Entities;

/// <summary>
/// Common Vulnerabilities and Exposures
/// </summary>
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

	/// <summary>
	/// CVE-XXXX representation. Primary key.
	/// </summary>
	[Column("year")]
	public int Year { get; set; }
	/// <summary>
	/// CVE-...-XXXX representation. Primary key.
	/// </summary>
	[Column("cna_number"), MaxLength(20)]
	public string CnaNumber { get; set; }

	[NotMapped] public CveId CveId => new(Year, CnaNumber);

	/// <summary>
	/// Description of CVE properties and effects
	/// </summary>
	[Column("description")]
	public string? Description { get; init; }

	/// <summary>
	/// Common Vulnerability Scoring System
	/// </summary>
	[Column("cvss")]
	public double? CVSS { get; init; }

	public ICollection<VulnerabilityPointEntity> Vulnerabilities { get; set; } = new List<VulnerabilityPointEntity>();

	public override string ToString() => CveId.ToString();
}