namespace KutCode.Cve.Application.Interfaces;

/// <summary>
/// Manage Queue of cve loading
/// </summary>
public interface IFinderQueueManager
{
	Task AddRangeAsync(List<CveFinderQueueEntity> loadRequest, CancellationToken ct = default);
	Task AddAsync(CveFinderQueueEntity loadRequest, CancellationToken ct = default);
	Task AddByYearAsync(int cveYear, string finder,  int priority = 0, CancellationToken ct = default);
	Task RemoveAsync(CveFinderQueueEntity loadRequest, CancellationToken ct = default);
	Task RemoveRangeAsync(List<CveFinderQueueEntity> loadRequest, CancellationToken ct = default);
	Task<List<CveFinderQueueEntity>> GetNextAsync(int count, CancellationToken ct = default);
	Task<CveFinderQueueState> GetStateAsync(CancellationToken ct = default);
	Task FlushQueueAsync(CancellationToken ct = default);
}