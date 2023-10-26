using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto;

namespace KutCode.Cve.Api.Endpoints.Queue.GetQueueState;

public class Endpoint : EndpointWithoutRequest<CveResolverQueueState>
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
		Get("queue/resolve");
	}

	public override async Task<CveResolverQueueState> ExecuteAsync(CancellationToken ct)
	{
		return await _manager.GetStateAsync(ct);
	}
}