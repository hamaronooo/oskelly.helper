using FastEndpoints;
using KutCode.Cve.Application.CQRS.Cve;
using KutCode.Cve.Services.Loaders;
using MediatR;

namespace KutCode.Cve.Api.Endpoints.Cve.CreateByYear;

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
		Post("cve/load/mitre/byYear");
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		await _mediator.Send(new EditOrCreateCveFromMitreCommand(req.Year, new MitreYearlyLoader()), ct);
		await SendOkAsync(ct);
	}
}