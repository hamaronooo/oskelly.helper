﻿using KutCode.Cve.Application.Interfaces;

namespace KutCode.Cve.Api.Endpoints.Cve.Resolve.YearResolve;

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
		Post("cve/resolve/year");
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		await _manager.AddByYearAsync(req, ct);
		await SendOkAsync(ct);
	}
}