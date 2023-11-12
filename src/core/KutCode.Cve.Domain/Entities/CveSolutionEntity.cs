using System.ComponentModel.DataAnnotations.Schema;
using KutCode.Cve.Domain.Entities.Base;

namespace KutCode.Cve.Domain.Entities;

/// <summary>
/// CveSolutionEntity of CVE
/// </summary>
[Table("cve_solution")]
public sealed class CveSolutionEntity : ModelWithId<Guid>
{
	public CveSolutionEntity() { }
	public CveSolutionEntity(Guid uid) : base(uid) { }
	
	[Column("info")]
	public string Info { get; set; }
	
	/// <summary>
	/// Text description of solution
	/// </summary>
	[Column("description")]
	public string? Description { get; set; }

	[Column("solution_link")]
	public string? SolutionLink { get; set; }
	
	[Column("download_link")]
	public string? DownloadLink { get; set; }
	
	[Column("additional_link")]
	public string? AdditionalLink { get; set; }

	[Column("vulnerability_point_id")]
	public Guid VulnerabilityPointId { get; set; }
	public VulnerabilityPointEntity VulnerabilityPoint { get; set; }
	
	public override string ToString() => Info;
}