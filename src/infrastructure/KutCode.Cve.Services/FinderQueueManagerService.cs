using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Services;

public sealed class FinderQueueManagerService : IFinderQueueManager
{
	private readonly MainDbContext _context;

	public FinderQueueManagerService(MainDbContext context)
	{
		_context = context;
	}

	public async Task AddRangeAsync(List<CveFinderQueueEntity> loadRequest, CancellationToken ct = default)
	{
		await _context.CveFinderQueue.UpsertRange(loadRequest)
			.On(x => new { x.CveCnaNumber, x.CveYear, x.FinderCode })
			.NoUpdate()
			.RunAsync(ct);
	}

	public async Task AddAsync(CveFinderQueueEntity loadRequest, CancellationToken ct = default)
	{
		await _context.CveFinderQueue.Upsert(loadRequest)
			.On(x => new { x.CveCnaNumber, x.CveYear, x.FinderCode })
			.NoUpdate()
			.RunAsync(ct);
	}

	public async Task AddByYearAsync(int cveYear, string finder, int priority = 0, CancellationToken ct = default)
	{
		var entities = await _context.Cve
			.AsNoTracking()
			.Where(x => x.Year == cveYear)
			.Select(x => new CveFinderQueueEntity(x.Year, x.CnaNumber, finder, priority))
			.ToListAsync(ct);
		await _context.CveFinderQueue.UpsertRange(entities)
			.On(x => new { x.CveCnaNumber, x.CveYear, x.FinderCode })
			.NoUpdate()
			.RunAsync(ct);
	}

	public async Task RemoveAsync(CveFinderQueueEntity loadRequest, CancellationToken ct = default)
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

	public async Task RemoveRangeAsync(List<CveFinderQueueEntity> loadRequest, CancellationToken ct = default)
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

	public async Task<List<CveFinderQueueEntity>> GetNextAsync(int count, CancellationToken ct = default)
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