using KutCode.Cve.Application.Hubs;
using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace KutCode.Cve.Api.Services;

public sealed class WebsocketHandler : IWebsocketHandler
{
	private readonly IHubContext<ReportHub> _reportHub;
	public WebsocketHandler(IHubContext<ReportHub> reportHub)
	{
		_reportHub = reportHub;
	}

	public async Task SendReportStateAsync(Guid reportId, ReportRequestState state, CancellationToken ct = default)
	{
		await _reportHub.Clients.All.SendAsync("update", new { reportId, state }, ct);
	}
}