using KutCode.Cve.Domain.Dto.Websockets;
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Application.Interfaces;

public interface IWebsocketHandler
{
	Task SendReportStateAsync(ReportRequestStateUpdateDto message, CancellationToken ct = default);
}