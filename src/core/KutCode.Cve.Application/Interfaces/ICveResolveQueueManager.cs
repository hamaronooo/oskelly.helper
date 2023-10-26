using KutCode.Cve.Domain.Models.CveVulnerabilityLoader;

namespace KutCode.Cve.Application.Interfaces;

/// <summary>
/// Manage Queue of cve loading
/// </summary>
public interface ICveResolveQueueManager
{
	Task AddRangeAsync(List<CveResolveQueueEntity> loadRequest, CancellationToken ct = default);
	Task AddAsync(CveResolveQueueEntity loadRequest, CancellationToken ct = default);
	Task AddByYearAsync(YearCveVulnerabilityLoadRequest vulnerabilityLoadRequest, CancellationToken ct = default);
	Task RemoveAsync(CveResolveQueueEntity loadRequest, CancellationToken ct = default);
	Task RemoveRangeAsync(List<CveResolveQueueEntity> loadRequest, CancellationToken ct = default);
	Task<List<CveResolveQueueEntity>> GetNextAsync(int count, CancellationToken ct = default);
	Task<CveResolverQueueState> GetStateAsync(CancellationToken ct = default);
	Task FlushQueueAsync(CancellationToken ct = default);
}