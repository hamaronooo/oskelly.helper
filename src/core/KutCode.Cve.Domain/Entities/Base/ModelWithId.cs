using System.ComponentModel.DataAnnotations.Schema;

namespace KutCode.Cve.Domain.Entities.Base;

public abstract class ModelWithId<TId> where TId : struct 
{
	public ModelWithId() { }
	public ModelWithId(TId id)
	{
		Id = id;
	}
	
	[Column("id")]
	public TId Id { get; set; }
}