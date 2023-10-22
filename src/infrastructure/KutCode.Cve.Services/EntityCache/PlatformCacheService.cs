using KutCode.Cve.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services.EntityCache;

public sealed class PlatformCacheService : IEntityCacheService<PlatformEntity, Guid>
{
	private object _locker = new();
	private Dictionary<string, Guid> _cache;
	private readonly MainDbContext _context;
	public PlatformCacheService(IServiceScopeFactory scopeFactory)
	{
		_context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MainDbContext>();
		_cache = _context.Platforms.AsNoTracking().ToDictionary(x => x.Name.ToLower().Trim(), x => x.Id);
	}

	public Guid GetOrAddId(PlatformEntity platform)
	{
		var platformName = platform.Name.ToLower().Trim();
		lock (_locker)
		{
			if (_cache.TryGetValue(platformName, out var id))
				return id;
			_context.Platforms.Add(platform);
			_context.SaveChanges();
			_cache.Add(platformName, platform.Id);
		}
		return platform.Id;
	}
}