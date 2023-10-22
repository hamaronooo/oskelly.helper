using FastEndpoints;
using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Domain.Entities;
using KutCode.Cve.Domain.Models;

namespace KutCode.Cve.Api.Endpoints.Cve.FindResolves;

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
		Post("cve/resolve/find");
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		List<CveFinderQueueEntity> list = req.Items.Select(x
			=> new CveFinderQueueEntity(CveId.Parse(x.CveId), x.SourceCode, 10)).ToList();
		await _manager.AddRangeAsync(list, ct);
		await SendOkAsync(ct);
	}
}