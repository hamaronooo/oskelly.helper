using KutCode.Cve.Application.CveFinder;

namespace KutCode.Cve.Api.Hosted;

public class LoaderProcessorService : IHostedService
{
	private readonly CveFinderProcessor _processor;

	public LoaderProcessorService(CveFinderProcessor processor)
	{
		_processor = processor;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await Task.Factory.StartNew(async () => await _processor.StartAsync(cancellationToken), cancellationToken,
			TaskCreationOptions.LongRunning, TaskScheduler.Default);
	}

	public  Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}