using FastEndpoints;
using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.MQ;
using KutCode.Cve.Domain.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Api.Endpoints.Cve.FindYearlyResolves;

public class Endpoint : Endpoint<Request>
{
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly MainDbContext _context;

	public Endpoint(IPublishEndpoint publishEndpoint, MainDbContext context)
	{
		_publishEndpoint = publishEndpoint;
		_context = context;
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
		var cves = await _context.Cve.Where(x => x.Year == req.Year).Select(x => x.CveId).ToListAsync();
		foreach (var item in cves)
			await _publishEndpoint.Publish(new FindCveResolveMessage(item.Year, item.CnaNumber, req.SourceCode));
		await SendOkAsync(ct);
	}
}