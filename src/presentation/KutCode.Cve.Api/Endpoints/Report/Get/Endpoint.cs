
using KutCode.Cve.Application.CQRS.Report;
using KutCode.Cve.Domain.Dto.Entities.Report;

namespace KutCode.Cve.Api.Endpoints.Report.Get;

public sealed class Endpoint : EndpointWithoutRequest<ReportRequestExtendedDto>
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
		Get("report/{id}");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var id = Route<Guid>("id");
		var response = await _mediator.Send(new ExtendedReportByIdQuery(id));
		if (response.HasValue is false) ThrowError("Отчет не найден", 404);
		await SendOkAsync(response.Value!, ct);
	}
}