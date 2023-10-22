using KutCode.Cve.Application.Database;
using MediatR;
using Serilog;

namespace KutCode.Cve.Application.CQRS.Cve;

public sealed record FindCveResolveCommand(CveId CveId, string FinderCode) : IRequest;

public class FindCveResolveCommandHandler : IRequestHandler<FindCveResolveCommand>
{
	private readonly IResolveFinderManager _finderManager;
	private readonly ICveCache _cveCache;
	private readonly IEntityCacheService<SoftwareEntity, Guid> _softwareCache;
	private readonly IEntityCacheService<PlatformEntity, Guid> _platformCache;
	private readonly MainDbContext _context;

	public FindCveResolveCommandHandler(
		IResolveFinderManager finderManager, 
		IEntityCacheService<SoftwareEntity, Guid> softwareCache,
		IEntityCacheService<PlatformEntity, Guid> platformCache, 
		MainDbContext context,
        ICveCache cveCache)
	{
		_finderManager = finderManager;
		_softwareCache = softwareCache;
		_platformCache = platformCache;
		_context = context;
		_cveCache = cveCache;
	}

	public async Task Handle(FindCveResolveCommand request, CancellationToken ct)
	{
		var finder = _finderManager.GetFinder(request.FinderCode);
		if (finder is null) {
			Log.Error("{ClassName}; Finder with code {FCode} is not found", GetType().Name, request.FinderCode);
			return;
		}

		if (_cveCache.IsExist(request.CveId) is false) return;
		
		int addCounter = 0;
		var finderResult = await finder.FindAsync(request.CveId, ct);
		foreach (var found in finderResult)
		{
			if (found.Platform is not null)
			{
				found.PlatformId = _platformCache.GetOrAddId(found.Platform);
				found.Platform = null;
			}
			if (found.Software is not null)
			{
				found.SoftwareId = _softwareCache.GetOrAddId(found.Software);
				found.Software = null;
			}

			await _context.VulnerabilityPoints.AddAsync(found, ct);
			addCounter++;
		}
		
		await _context.SaveChangesAsync(ct);
		Log.Information("Saved {Count} vul-points for CVE {CVE}; with finder code: {FCode}",
			addCounter, request.CveId, request.FinderCode);
	}
}