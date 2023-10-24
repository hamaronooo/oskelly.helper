using KutCode.Cve.Application.Interfaces;

namespace KutCode.Cve.Api.Endpoints.Queue.FlushQueue;

public class Endpoint : EndpointWithoutRequest
{
	private readonly ICveResolveQueueManager _manager;

	public Endpoint(ICveResolveQueueManager manager)
	{
		_manager = manager;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("queue/flush/resolve");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		await _manager.FlushQueueAsync(ct);
		await SendOkAsync(ct);
	}
}