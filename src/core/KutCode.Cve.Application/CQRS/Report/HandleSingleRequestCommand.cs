using System.Collections.Concurrent;
using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Enums;
using KutCode.Cve.Domain.Models.Solution;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record HandleSingleRequestCommand(ReportRequestExtendedDto ReportRequest) : IRequest<SolutionFinderResult<VulnerabilityPointEntity>>;
public class HandleSingleRequestCommandHandler : IRequestHandler<HandleSingleRequestCommand,SolutionFinderResult<VulnerabilityPointEntity>>
{
	private readonly IMediator _mediator;
	private readonly MainDbContext _context;
	private readonly ICveResolverManager _vulnerabilityLoaderManager;
	private readonly ICveSolutionFinder _solutionFinder;

	public HandleSingleRequestCommandHandler(
		IMediator mediator, 
		MainDbContext context,
		ICveResolverManager vulnerabilityLoaderManager,
		ICveSolutionFinder solutionFinder)
	{
		_mediator = mediator;
		_context = context;
		_vulnerabilityLoaderManager = vulnerabilityLoaderManager;
		_solutionFinder = solutionFinder;
	}

	public async Task<SolutionFinderResult<VulnerabilityPointEntity>> Handle(HandleSingleRequestCommand request, CancellationToken ct)
	{ 
		List<VulnerabilityPointEntity> resolversResults = new();
		var rReq = request.ReportRequest;
		if (rReq.SearchStrategy == ReportSearchStrategy.Combine) {
			resolversResults.AddRange(await GetResolvesResult(rReq, ct));
			resolversResults.AddRange(await GetDbResolves(rReq, ct));
		}
		if (rReq.SearchStrategy == ReportSearchStrategy.OnlyNew)
			resolversResults.AddRange(await GetResolvesResult(rReq!, ct));
		if (rReq.SearchStrategy == ReportSearchStrategy.OnlyStorage)
			resolversResults.AddRange(await GetDbResolves(rReq!, ct));
		
		return await _solutionFinder.FindAsync(rReq.Vulnerabilities.Single(), resolversResults, new () { ShowResultsIfEmptyPrompt = true }, ct);
	}
	
	#region Help methods

	private async Task<List<VulnerabilityPointEntity>> GetDbResolves(ReportRequestExtendedDto rReqValue, CancellationToken ct)
	{
		List<VulnerabilityPointEntity> result = new(rReqValue.Vulnerabilities.Count);
		foreach (var vulnerability in rReqValue.Vulnerabilities)
		{
			var resolves = await _context.VulnerabilityPoints.AsNoTracking()
				.Include(x => x.Platform)
				.Include(x => x.Software)
				.Include(x => x.CveSolutions)
				.Where(x => x.CveYear == vulnerability.CveYear
				            && x.CveCnaNumber == vulnerability.CveCnaNumber)
				.ToListAsync(ct);
			result.AddRange(resolves);
		}
		return result;
	}


	private async Task<List<VulnerabilityPointEntity>> GetResolvesResult(ReportRequestExtendedDto rReq, CancellationToken ct)
	{
		List<ICveResolver> resolvers = new(rReq.Sources.Length);
		foreach (var sourceCode in rReq.Sources) {
			var resolver = _vulnerabilityLoaderManager.GetResolver(sourceCode.Trim());
			if (resolver.HasValue) resolvers.Add(resolver.Value!);
		}

		var parallelOptions = new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = 20 };
		var bag = new ConcurrentBag<VulnerabilityPointEntity>();
		await Parallel.ForEachAsync(rReq.Vulnerabilities, parallelOptions, async (dto, token) => {
			foreach (var resolver in resolvers) {
				try {
					var resolveResults = await resolver.ResolveAsync(new CveId(dto.CveYear, dto.CveCnaNumber), token);
					foreach (var result in resolveResults)
					{
						result.CveYear = dto.CveYear;
						result.CveCnaNumber = dto.CveCnaNumber;
						result.Description = dto.CveDescription ?? result.Description;
						result.Platform ??= new PlatformEntity() { Name = string.Empty };
						result.Software ??= new SoftwareEntity() { Name = string.Empty };
						bag.Add(result);
					}
				}
				catch (Exception e) {
					Log.Error(e, "Resolver Error; Resolver code: {Code}; CveString: {Cve}", resolver.Code, dto.CveString);
					// swallow
				}
			}
		});
		return bag.ToList();
	}

	#endregion
}