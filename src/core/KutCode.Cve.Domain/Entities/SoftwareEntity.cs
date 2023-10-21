using System.ComponentModel.DataAnnotations.Schema;
using KutCode.Cve.Domain.Entities.Base;

namespace KutCode.Cve.Domain.Entities;

/// <summary>
/// Computer program
/// </summary>
[Table("software")]
public sealed class SoftwareEntity : ModelWithId<Guid>
{
	public SoftwareEntity() { }
	public SoftwareEntity(Guid uid) : base(uid) { }
	
	[Column("name")]
	public string Name { get; init; }
	
	public ICollection<VulnerabilityPointEntity> VulnerabilityPoints { get; set; }

	public override string ToString() => Name;
}