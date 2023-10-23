using KutCode.Cve.Domain.Models.CveResolver;

namespace KutCode.Cve.Application.Interfaces;

/// <summary>
/// Manage Queue of cve loading
/// </summary>
public interface ICveResolveQueueManager
{
	Task AddRangeAsync(List<CveResolveQueueEntity> loadRequest, CancellationToken ct = default);
	Task AddAsync(CveResolveQueueEntity loadRequest, CancellationToken ct = default);
	Task AddByYearAsync(YearCveResolveRequest resolveRequest, CancellationToken ct = default);
	Task RemoveAsync(CveResolveQueueEntity loadRequest, CancellationToken ct = default);
	Task RemoveRangeAsync(List<CveResolveQueueEntity> loadRequest, CancellationToken ct = default);
	Task<List<CveResolveQueueEntity>> GetNextAsync(int count, CancellationToken ct = default);
	Task<CveFinderQueueState> GetStateAsync(CancellationToken ct = default);
	Task FlushQueueAsync(CancellationToken ct = default);
}