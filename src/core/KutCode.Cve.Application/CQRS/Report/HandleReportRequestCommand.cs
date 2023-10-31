using System.Collections.Concurrent;
using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Application.Interfaces.Excel;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Enums;
using KutCode.Cve.Domain.Models.Solution;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record HandleReportRequestCommand(Guid RequestId) : IRequest;

public sealed class HandleReportRequestCommandHandler: IRequestHandler<HandleReportRequestCommand>
{
	private readonly IMediator _mediator;
	private readonly MainDbContext _context;
	private readonly ICveResolverManager _vulnerabilityLoaderManager;
	private readonly ICveSolutionFinder _solutionFinder;
	private readonly ICveReportCreator _reportCreator;
	private readonly IFileService _fileService;

	public HandleReportRequestCommandHandler(
		IMediator mediator,
		ICveResolverManager vulnerabilityLoaderManager,
		MainDbContext context,
		ICveSolutionFinder solutionFinder,
		ICveReportCreator reportCreator, 
		IFileService fileService)
	{
		_mediator = mediator;
		_vulnerabilityLoaderManager = vulnerabilityLoaderManager;
		_context = context;
		_solutionFinder = solutionFinder;
		_reportCreator = reportCreator;
		_fileService = fileService;
	}

	public async Task Handle(HandleReportRequestCommand request, CancellationToken ct)
	{
		Optional<ReportRequestExtendedDto> rReq = await _mediator.Send(new ExtendedReportByIdQuery(request.RequestId), ct);
		if (rReq.HasValue is false) {
			Log.Warning("{ClassName}; Load request with Id: {Id} not found", GetType().Name, request.RequestId);
			return;
		}
		await _mediator.Send(new ChangeReportRequestStateCommand(rReq.Value!.Id, ReportRequestState.Handling), ct);
		
		try {
			await Wrapper(rReq.Value!, ct);
		}
		catch {
			await _mediator.Send(new ChangeReportRequestStateCommand(rReq.Value!.Id, ReportRequestState.Error), ct);
			throw;
		}
	}
	
	public async Task Wrapper(ReportRequestExtendedDto rReq, CancellationToken ct)
	{
		// load resolves 
		List<VulnerabilityPointEntity> resolversResults = new();
		Log.Information("{ClassName}; ReportSearchStrategy for request with Id: {Id} is {SearchStrategy}", 
			GetType().Name, rReq.Id, rReq.SearchStrategyName);
		if (rReq.SearchStrategy == ReportSearchStrategy.Combine) {
			resolversResults.AddRange(await GetResolvesResult(rReq!, ct));
			resolversResults.AddRange(await GetDbResolves(rReq!, ct));
		}
		if (rReq.SearchStrategy == ReportSearchStrategy.OnlyNew)
			resolversResults.AddRange(await GetResolvesResult(rReq!, ct));
		if (rReq.SearchStrategy == ReportSearchStrategy.OnlyStorage)
			resolversResults.AddRange(await GetDbResolves(rReq!, ct));
		
		Log.Information("{ClassName}; Loaded {Count} resolves for report request: {Id}", 
			GetType().Name, resolversResults.Count, rReq.Id);
		
		// joining cve request with loaded resolve
		var solutionSearchSet = rReq.Vulnerabilities
			.Join(resolversResults.GroupBy(x => x.CveId),
				requested => requested.CveId, 
				loadedResolves => loadedResolves.Key,
				(req, res) => new {
					RequestedVulnerability = req, LoadedResolves = res
				});

		// find solutions 
		var parallelOptions = new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = 20 };
		var foundSolutions = new ConcurrentBag<SolutionFinderResult<VulnerabilityPointEntity>>();
		await Parallel.ForEachAsync(solutionSearchSet, parallelOptions, async (dto, token) => {
			try {
				SolutionFinderResult<VulnerabilityPointEntity> solutions = await _solutionFinder.FindAsync(dto.RequestedVulnerability, dto.LoadedResolves, token);
				foundSolutions.Add(solutions);
			}
			catch {
				//swallow
			}
		});

		Log.Information("{ClassName}; Found {Count} solutions for report request: {Id}", 
			GetType().Name, foundSolutions.Count, rReq.Id);
		
		// create and save report 
		var reportBytes = await _reportCreator.CreateExcelReportAsync(rReq, foundSolutions, ct);
		await _fileService.SaveFileAsync(reportBytes, rReq.Id, ct);
		
		Log.Information("{ClassName}; Report with Id: {Id}, saved SUCCESSFULLY", GetType().Name, rReq.Id);
		await _mediator.Send(new ChangeReportRequestStateCommand(rReq.Id, ReportRequestState.Success), ct);
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
					var resolveResult = await resolver.ResolveAsync(new CveId(dto.CveYear, dto.CveCnaNumber), token);
					foreach (var result in resolveResult)
					{
						result.CveYear = dto.CveYear;
						result.CveCnaNumber = dto.CveCnaNumber;
						result.Description = dto.CveDescription;
						result.Platform ??= new PlatformEntity() { Name = dto.Platform ?? string.Empty };
						result.Software ??= new SoftwareEntity() { Name = dto.Software ?? string.Empty };
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