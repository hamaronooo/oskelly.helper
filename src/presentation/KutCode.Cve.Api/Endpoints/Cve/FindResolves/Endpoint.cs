using FastEndpoints;
using KutCode.Cve.Application.MQ;
using KutCode.Cve.Domain.Models;
using MassTransit;

namespace KutCode.Cve.Api.Endpoints.Cve.FindResolves;

public class Endpoint : Endpoint<Request>
{
	private readonly IPublishEndpoint _publishEndpoint;

	public Endpoint(IPublishEndpoint publishEndpoint)
	{
		_publishEndpoint = publishEndpoint;
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
		foreach (var item in req.Items)
		{
			var id = CveId.Parse(item.CveId);
			await _publishEndpoint.Publish(new FindCveResolveMessage(id.Year, id.CnaNumber, item.SourceCode));
		}
		await SendOkAsync(ct);
	}
}