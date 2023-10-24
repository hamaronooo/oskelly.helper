using System.Diagnostics;
using KutCode.Cve.Application.CQRS.Cve;
using KutCode.Cve.Domain.Models.CveResolver;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KutCode.Cve.Application.CveResolve;

public class CveResolveProcessor
{
	private readonly IMediator _mediatr;
	private readonly ICveResolveQueueManager _cveResolveQueueManager;
	private const int LoadBunchCount = 100;
	public CveResolveProcessor(IServiceScopeFactory scopeFactory)
	{
		var scope = scopeFactory.CreateScope();
		_cveResolveQueueManager = scope.ServiceProvider.GetRequiredService<ICveResolveQueueManager>();
		_mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

	}
	
	public async Task StartAsync(CancellationToken ct)
	{
		while (ct.IsCancellationRequested == false)
		{
			try
			{
				await LoadNextAsync(ct);
			}
			catch
			{
				// swallow 
			}
			await Task.Delay(1000, ct);
		}
	}

	private async Task LoadNextAsync(CancellationToken ct)
	{
		var next = await _cveResolveQueueManager.GetNextAsync(LoadBunchCount);
		if (next.Count == 0) return;
		var ts1 = Stopwatch.GetTimestamp();
		foreach (var nextItem in next)
		{
			Log.Information("{ClassName}; Start finding resolves for CVE: {Cve} with finder code: {FCode}", 
				GetType().Name, nextItem.CveId, nextItem.ResolverCode);
			try {
				await _mediatr.Send(new ResolveCveCommand(new SingleCveResolveRequest {
					CveId = nextItem.CveId,
					Priority = nextItem.Priority,
					ResolverCode = nextItem.ResolverCode,
					UpdateCve = nextItem.UpdateCve
				}), ct);
			}
			catch (Exception e) {
				Log.Error(e, "Error in cve finder work, cve: {CveId}; finder: {FinderCode}", nextItem.CveId, nextItem.ResolverCode);
			}
		}
		
		Log.Information("{ClassName}; Loaded {Count} CVE for {Elapsed} time", 
			GetType().Name, LoadBunchCount, Stopwatch.GetElapsedTime(ts1, Stopwatch.GetTimestamp()));
		await _cveResolveQueueManager.RemoveRangeAsync(next, ct);
	}
}