using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Application.Interfaces;

public interface IWebsocketHandler
{
	Task SendReportStateAsync(Guid reportId, ReportRequestState state, CancellationToken ct = default);
}