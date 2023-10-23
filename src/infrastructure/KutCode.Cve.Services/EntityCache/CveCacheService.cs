using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services.EntityCache;

public sealed class CveCacheService : ICveCache
{
	private readonly HashSet<CveId> _hash;
	private readonly object _locker = new();

	public CveCacheService(IServiceScopeFactory scopeFactory)
	{
		MainDbContext context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MainDbContext>();
		_hash = context.Cve.AsNoTracking().Select(x => new CveId(x.Year, x.CnaNumber))
			.ToHashSet();
	}

	public bool IsExist(CveId cveId)
	{
		lock (_locker)
		{
			return _hash.Contains(cveId);
		}
	}

	public void Add(CveId cveId)
	{
		lock (_locker)
		{
			_hash.Add(cveId);
		}
	}

	public void AddRange(IEnumerable<CveId> cve)
	{
		lock (_locker)
			foreach (var i in cve) _hash.Add(i);
	}
}