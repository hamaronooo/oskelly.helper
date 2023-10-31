using KutCode.Cve.Api.Hubs;
using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace KutCode.Cve.Api.Services;

public sealed class WebsocketHandler : IWebsocketHandler
{
	private readonly IHubContext<WsHub> _reportHub;
	public WebsocketHandler(IHubContext<WsHub> reportHub)
	{
		_reportHub = reportHub;
	}

	public async Task SendReportStateAsync(Guid reportId, ReportRequestState state, CancellationToken ct = default)
	{
		var json = JsonConvert.SerializeObject(new {reportId, state, stateName = EnumHelper.GetDescriptionValue(state)}, Formatting.None);
		await _reportHub.Clients.All.SendAsync("update_report_state", json, ct);
	}
}