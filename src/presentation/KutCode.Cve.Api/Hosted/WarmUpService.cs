using KutCode.Cve.Application.Database;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Api.Hosted;

/// <summary>
/// Warm up application and do basic startup work on application start
/// </summary>
public sealed class WarmUpService : IHostedService
{
	private readonly MainDbContext _context;
	public WarmUpService(IServiceScopeFactory scopeFactory)
	{
		var scope = scopeFactory.CreateScope();
		_context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
	}
	
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await _context.Database.MigrateAsync(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}