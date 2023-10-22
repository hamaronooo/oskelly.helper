using KutCode.Cve.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services.EntityCache;

public sealed class SoftwareCacheService : IEntityCacheService<SoftwareEntity, Guid>
{
	private object _locker = new();
	private Dictionary<string, Guid> _cache;
	private readonly MainDbContext _context;
	public SoftwareCacheService(IServiceScopeFactory scopeFactory)
	{
		_context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MainDbContext>();
		_cache = _context.Software.AsNoTracking().ToDictionary(x => x.Name.ToLower().Trim(), x => x.Id);
	}

	public Guid GetOrAddId(SoftwareEntity software)
	{
		var softwareName = software.Name.ToLower().Trim();
		lock (_locker)
		{
			if (_cache.TryGetValue(softwareName, out var id))
				return id;
			_context.Software.Add(software);
			_context.SaveChanges();
			_cache.Add(softwareName, software.Id);
		}
		return software.Id;
	}
}