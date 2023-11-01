using KutCode.Cve.Api.Hubs;
using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Domain.Dto.Websockets;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KutCode.Cve.Api.Services;

public sealed class WebsocketHandler : IWebsocketHandler
{
	private readonly IHubContext<WsHub> _reportHub;
	private static JsonSerializerSettings SerializerSettings = new () {
		ContractResolver = new CamelCasePropertyNamesContractResolver()
	};

	public WebsocketHandler(IHubContext<WsHub> reportHub)
	{
		_reportHub = reportHub;
	}

	public async Task SendReportStateAsync(ReportRequestStateUpdateDto message, CancellationToken ct = default)
	{
		
		var json = JsonConvert.SerializeObject(message, SerializerSettings);
		await _reportHub.Clients.All.SendAsync("update_report_state", json, ct);
	}
}