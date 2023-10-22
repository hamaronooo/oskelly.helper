using FastEndpoints;
using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Application.MQ;

namespace KutCode.Cve.Api.Endpoints.Cve.FindYearlyResolves;

public class Endpoint : Endpoint<Request>
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
		Post("cve/resolve/find/yearly");
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		await _manager.AddByYearAsync(req.Year, req.SourceCode, 0);
		await SendOkAsync(ct);
	}
}