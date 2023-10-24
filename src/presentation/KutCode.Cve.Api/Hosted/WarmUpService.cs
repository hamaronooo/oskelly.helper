namespace KutCode.Cve.Api.Hosted;

/// <summary>
///     Warm up application and do basic startup work on application start
/// </summary>
public sealed class WarmUpService : IHostedService
{
	public WarmUpService(IServiceScopeFactory scopeFactory) { }

	public Task StartAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}