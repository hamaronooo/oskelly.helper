using System.Collections.Concurrent;
using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities.Report;
using KutCode.Cve.Domain.Enums;
using KutCode.Cve.Domain.Models.Solution;
using Serilog;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record HandleReportRequestCommand(Guid RequestId) : IRequest<IEnumerable<VulnerabilityPointEntity>>;

public sealed class HandleReportRequestCommandHandler: IRequestHandler<HandleReportRequestCommand, IEnumerable<VulnerabilityPointEntity>>
{
	private readonly IMediator _mediator;
	private readonly MainDbContext _context;
	private readonly ICveResolverManager _vulnerabilityLoaderManager;
	private readonly ICveSolutionFinder _solutionFinder;

	public HandleReportRequestCommandHandler(
		IMediator mediator,
		ICveResolverManager vulnerabilityLoaderManager,
		MainDbContext context,
		ICveSolutionFinder solutionFinder)
	{
		_mediator = mediator;
		_vulnerabilityLoaderManager = vulnerabilityLoaderManager;
		_context = context;
		_solutionFinder = solutionFinder;
	}

	public async Task<IEnumerable<VulnerabilityPointEntity>> Handle(HandleReportRequestCommand request, CancellationToken ct)
	{
		Optional<ReportRequestExtendedDto> rReq = await _mediator.Send(new ExtendedReportByIdQuery(request.RequestId), ct);
		if (rReq.HasValue is false) return Enumerable.Empty<VulnerabilityPointEntity>();

		_context.ReportRequests
			.Attach(new ReportRequestEntity {Id = rReq.Value!.Id, State = ReportRequestState.Handling})
			.Property(x => x.State).IsModified = true;
		await _context.SaveChangesAsync(ct);

		List<VulnerabilityPointEntity> resolversResults = new();
		//if (rReq.Value.SearchStrategy.)
		resolversResults.AddRange(await GetResolvesResult(rReq.Value!, ct));

		// search in DB if need OR/AND
		// get resolvers by codes
		
		//rReq.Value.Vulnerabilities

		var solutionSearchSet = rReq.Value.Vulnerabilities
			.Join(resolversResults.GroupBy(x => x.CveId),
				requested => requested.CveId, 
				loadedResolves => loadedResolves.Key,
				(req, res) => new {
					RequestedVulnerability = req, LoadedResolves = res
				});

		var parallelOptions = new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = 20 };
		var bag = new ConcurrentBag<SolutionFinderResult<VulnerabilityPointEntity>>();

		foreach (var s in solutionSearchSet)
		{
			try
			{
				SolutionFinderResult<VulnerabilityPointEntity> solutions =
					await _solutionFinder.FindAsync(s.RequestedVulnerability, s.LoadedResolves, ct);
				bag.Add(solutions);
			}
			catch (Exception e)
			{
				//swallow
			}
		}
		
		// await Parallel.ForEachAsync(solutionSearchSet, parallelOptions, async (dto, token) => {
		// 	SolutionFinderResult<VulnerabilityPointEntity> solutions = await _solutionFinder.FindAsync(dto.RequestedVulnerability, dto.LoadedResolves, token);
		// 	bag.Add(solutions);
		// });
		

		// combine resolves and pick best

		// publish update CVE queue in need and 

		// return result 
		
		// now selecting top 1 by score for each CVE
		return bag.Where(x => x.Best.HasValue)
			.Select(x => x.Best.Value!);
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
					var resolveResult = await resolver.ResolveAsync(new CveId(dto.CveYear, dto.CveCnaNumber), token);
					foreach (var result in resolveResult)
						bag.Add(result);
				}
				catch (Exception e) {
					Log.Error(e, "Resolver Error; Resolver code: {Code}; CveString: {Cve}", resolver.Code, dto.CveString);
					// swallow
				}
			}
		});
		return bag.ToList();
	}
}