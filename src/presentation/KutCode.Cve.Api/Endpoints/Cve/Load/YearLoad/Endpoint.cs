using FastEndpoints;
using KutCode.Cve.Application.CQRS.Cve;
using MediatR;

namespace KutCode.Cve.Api.Endpoints.Cve.Load.YearLoad;

public class Endpoint : Endpoint<Request>
{
	private readonly IMediator _mediator;

	public Endpoint(IMediator mediator)
	{
		_mediator = mediator;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("cve/load/year");
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		await _mediator.Send(new LoadCveCommand(req), ct);
		await SendOkAsync(ct);
	}
}