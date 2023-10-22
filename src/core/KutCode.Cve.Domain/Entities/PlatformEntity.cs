using System.ComponentModel.DataAnnotations.Schema;
using KutCode.Cve.Domain.Entities.Base;
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Entities;

/// <summary>
/// Operation system
/// </summary>
[Table("platform")]
public sealed class PlatformEntity : ModelWithId<Guid>
{
	public PlatformEntity() { }
	public PlatformEntity(Guid uid) : base(uid) {}
	
	[Column("name")]
	public string Name { get; set; }
	[Column("platform_type")]
	public PlatformType PlatformType { get; set; } = PlatformType.Unknown;

	public ICollection<VulnerabilityPointEntity> VulnerabilityPoints { get; set; }
	
	public override string ToString() => Name;
}