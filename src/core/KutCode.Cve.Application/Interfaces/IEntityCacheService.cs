namespace KutCode.Cve.Application.Interfaces;

public interface IEntityCacheService<TEntity, TKey> where TEntity : class, new()
{
	TKey GetOrAddId(TEntity entity);
}