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
	
	[Column("name")]
	public string Name { get; init; }
	
	/// <summary>
	/// Text description of solution
	/// </summary>
	[Column("description")]
	public string? Description { get; init; }

	[Column("solution_link")]
	public string? SolutionLink { get; init; }
	
	[Column("additional_link")]
	public string? AdditionalLink { get; init; }

	[Column("vulnerability_point_id")]
	public Guid VulnerabilityPointId { get; set; }
	public VulnerabilityPointEntity VulnerabilityPoint { get; set; }
	
	public override string ToString() => Name;
}