using System.Collections.Concurrent;
using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities.Report;
using KutCode.Cve.Domain.Enums;
using Serilog;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record HandleReportRequestCommand(Guid RequestId) : IRequest;

public sealed class HandleReportRequestCommandHandler: IRequestHandler<HandleReportRequestCommand>
{
	private readonly IMediator _mediator;
	private readonly MainDbContext _context;
	private readonly ICveResolverManager _vulnerabilityLoaderManager;

	public HandleReportRequestCommandHandler(IMediator mediator, ICveResolverManager vulnerabilityLoaderManager, MainDbContext context)
	{
		_mediator = mediator;
		_vulnerabilityLoaderManager = vulnerabilityLoaderManager;
		_context = context;
	}

	public async Task Handle(HandleReportRequestCommand request, CancellationToken ct)
	{
		Optional<ReportRequestExtendedDto> rReq = await _mediator.Send(new ExtendedReportByIdQuery(request.RequestId), ct);
		if (rReq.HasValue is false) return;

		_context.ReportRequests
			.Attach(new ReportRequestEntity {Id = rReq.Value!.Id, State = ReportRequestState.Handling})
			.Property(x => x.State).IsModified = true;
		await _context.SaveChangesAsync(ct);

		List<VulnerabilityPointEntity> resolversResults = new();
		//if (rReq.Value.SearchStrategy.)

		resolversResults.AddRange(await GetResolvesResult(rReq.Value!, ct));


		// search in DB if need OR/AND
		// get resolvers by codes

		// execute resolvers in parrallel

		// combine resolves and pick best

		// publish update CVE queue in need and 

		// return result 
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
		await Parallel.ForEachAsync(rReq.Cve, parallelOptions, async (dto, token) => {
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