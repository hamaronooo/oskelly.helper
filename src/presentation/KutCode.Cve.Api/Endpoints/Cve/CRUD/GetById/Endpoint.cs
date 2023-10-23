using FastEndpoints;
using KutCode.Cve.Application.CQRS.Cve;
using KutCode.Cve.Domain.Dto.Entities;
using KutCode.Cve.Domain.Models;
using MediatR;

namespace KutCode.Cve.Api.Endpoints.Cve.CRUD.GetById;

public class Endpoint : EndpointWithoutRequest<CveDto>
{
	private readonly IMediator _mediator;

	public Endpoint(IMediator mediator)
	{
		_mediator = mediator;
	}

	public override void Configure()
	{
		Version(1);
		AllowAnonymous();
		Get("cve/byId/{CveId}");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var cveId = CveId.Parse(Route<string>("CveId"));
		var result = await _mediator.Send(new GetCveByIdQuery(cveId), ct);
		if (result is null) ThrowError("CVE not found", 404);
		await SendOkAsync(result, ct);
	}
}