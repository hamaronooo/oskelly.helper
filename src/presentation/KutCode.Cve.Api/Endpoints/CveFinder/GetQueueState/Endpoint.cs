using FastEndpoints;
using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Domain.Dto;

namespace KutCode.Cve.Api.Endpoints.CveFinder.GetQueueState;

public class Endpoint : EndpointWithoutRequest<CveFinderQueueState>
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
		Get("cve/finder/queue");
	}

	public override async Task<CveFinderQueueState> ExecuteAsync(CancellationToken ct)
	{
		return await _manager.GetStateAsync(ct);	
	}
}