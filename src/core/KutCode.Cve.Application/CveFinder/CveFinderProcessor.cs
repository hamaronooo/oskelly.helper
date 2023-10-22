using KutCode.Cve.Application.CQRS.Cve;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KutCode.Cve.Application.CveFinder;

public class CveFinderProcessor
{
	private readonly IMediator _mediatr;
	private readonly IFinderQueueManager _finderQueueManager;
	public CveFinderProcessor(IServiceScopeFactory scopeFactory)
	{
		var scope = scopeFactory.CreateScope();
		_finderQueueManager = scope.ServiceProvider.GetRequiredService<IFinderQueueManager>();
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
			await Task.Delay(500, ct);
		}
	}

	private async Task LoadNextAsync(CancellationToken ct)
	{
		var next = await _finderQueueManager.GetNextAsync(100);
		foreach (var nextItem in next) {
			Log.Information("{ClassName}; Start finding resolves for CVE: {Cve} with finder code: {FCode}", 
				GetType().Name, nextItem.CveId, nextItem.FinderCode);
			try {
				await _mediatr.Send(new FindCveResolveCommand(nextItem.CveId, nextItem.FinderCode), ct);
			}
			catch (Exception e) {
				Log.Error(e, "Error in cve finder work, cve: {CveId}; finder: {FinderCode}", nextItem.CveId, nextItem.FinderCode);
			}
		}
		await _finderQueueManager.RemoveRangeAsync(next, ct);
	}
}