using FastEndpoints;
using KutCode.Cve.Application.Interfaces;

namespace KutCode.Cve.Api.Endpoints.CveFinder.FlushQueue;

public class Endpoint : EndpointWithoutRequest
{
	private readonly IFinderQueueManager _manager;

	public Endpoint(IFinderQueueManager manager)
	{
		_manager = manager;
	}
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("cve/finder/queue/flush");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		await _manager.FlushQueueAsync(ct);
		await SendOkAsync(ct);
	}
}