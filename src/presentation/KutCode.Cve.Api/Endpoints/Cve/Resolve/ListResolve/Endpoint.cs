using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Domain.Entities;
using KutCode.Cve.Domain.Models;

namespace KutCode.Cve.Api.Endpoints.Cve.Resolve.ListResolve;

public class Endpoint : Endpoint<Request>
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
		Post("cve/resolve/list");
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var list = req.CveList.Select(x
			=> new CveResolveQueueEntity(CveId.Parse(x), req.ResolverCode, 10) {
				UpdateCve = req.UpdateCve
			}).ToList();
		await _manager.AddRangeAsync(list, ct);
		await SendOkAsync(ct);
	}
}