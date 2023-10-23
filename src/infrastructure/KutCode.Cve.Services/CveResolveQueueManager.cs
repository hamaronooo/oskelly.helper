using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Dto;
using KutCode.Cve.Domain.Models.CveResolver;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Services;

public sealed class CveResolveQueueManager : ICveResolveQueueManager
{
	private readonly MainDbContext _context;

	public CveResolveQueueManager(MainDbContext context)
	{
		_context = context;
	}

	public async Task AddRangeAsync(List<CveResolveQueueEntity> loadRequest, CancellationToken ct = default)
	{
		await _context.CveFinderQueue.UpsertRange(loadRequest)
			.On(x => new { x.CveCnaNumber, x.CveYear, FinderCode = x.ResolverCode })
			.NoUpdate()
			.RunAsync(ct);
	}

	public async Task AddAsync(CveResolveQueueEntity loadRequest, CancellationToken ct = default)
	{
		await _context.CveFinderQueue.Upsert(loadRequest)
			.On(x => new { x.CveCnaNumber, x.CveYear, FinderCode = x.ResolverCode })
			.NoUpdate()
			.RunAsync(ct);
	}

	public async Task AddByYearAsync(YearCveResolveRequest resolveRequest, CancellationToken ct = default)
	{
		var entities = await _context.Cve
			.AsNoTracking()
			.Where(x => x.Year == resolveRequest.Year)
			.Select(x => new CveResolveQueueEntity(x.Year, x.CnaNumber, resolveRequest.ResolverCode, resolveRequest.Priority))
			.ToListAsync(ct);

		await _context.CveFinderQueue.UpsertRange(entities)
			.On(x => new { x.CveCnaNumber, x.CveYear, FinderCode = x.ResolverCode })
			.NoUpdate()
			.RunAsync(ct);
	}

	public async Task RemoveAsync(CveResolveQueueEntity loadRequest, CancellationToken ct = default)
	{
		try
		{
			_context.CveFinderQueue.Remove(loadRequest);
			await _context.SaveChangesAsync(ct);
		}
		catch
		{
			// swallow if flushed queue
		}
	}

	public async Task RemoveRangeAsync(List<CveResolveQueueEntity> loadRequest, CancellationToken ct = default)
	{
		try
		{
			_context.CveFinderQueue.RemoveRange(loadRequest);
			await _context.SaveChangesAsync(ct);
		}
		catch
		{
			// swallow if flushed queue
		}
	}

	public async Task<List<CveResolveQueueEntity>> GetNextAsync(int count, CancellationToken ct = default)
	{
		return await _context.CveFinderQueue.AsNoTracking()
			.OrderByDescending(x => x.Priority)
			.ThenBy(x => x.SysCreated)
			.Take(count)
			.ToListAsync(ct);
	}

	public async Task<CveFinderQueueState> GetStateAsync(CancellationToken ct = default)
	{
		var result = new CveFinderQueueState();
		result.Yearly = await _context.CveFinderQueue.AsNoTracking()
			.GroupBy(x => x.CveYear)
			.ToDictionaryAsync(x => x.Key, x => x.Count(), ct);
		return result;
	}

	public async Task FlushQueueAsync(CancellationToken ct = default)
	{
		await _context.CveFinderQueue.ExecuteDeleteAsync(ct);
		await _context.SaveChangesAsync(ct);
	}
}