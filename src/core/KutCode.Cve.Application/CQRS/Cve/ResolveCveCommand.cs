using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Models.CveResolver;
using MediatR;
using Serilog;

namespace KutCode.Cve.Application.CQRS.Cve;

public sealed record ResolveCveCommand(SingleCveResolveRequest ResolveRequest) : IRequest;

public class FindCveResolveCommandHandler : IRequestHandler<ResolveCveCommand>
{
	private readonly ICveResolverManager _finderManager;
	private readonly ICveCache _cveCache;
	private readonly IEntityCacheService<SoftwareEntity, Guid> _softwareCache;
	private readonly IEntityCacheService<PlatformEntity, Guid> _platformCache;
	private readonly MainDbContext _context;

	public FindCveResolveCommandHandler(
		ICveResolverManager finderManager, 
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

	public async Task Handle(ResolveCveCommand request, CancellationToken ct)
	{
		var resolver = _finderManager.GetResolver(request.ResolveRequest.ResolverCode);
		if (resolver is null) {
			Log.Error("{ClassName}; Resolver with code {RCode} is not found", GetType().Name, request.ResolveRequest.ResolverCode);
			return;
		}

		if (_cveCache.IsExist(request.ResolveRequest.CveId) is false) return;
		
		int addCounter = 0;
		var finderResult = await resolver.FindAsync(request.ResolveRequest.CveId, ct);
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
		Log.Information("Saved {Count} vul-points for CVE {Cve}; with finder code: {FCode}",
			addCounter, request.ResolveRequest.CveId, request.ResolveRequest.ResolverCode);
	}
}