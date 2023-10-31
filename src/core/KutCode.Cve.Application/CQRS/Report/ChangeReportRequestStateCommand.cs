using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record ChangeReportRequestStateCommand(Guid ReportRequestId, ReportRequestState NewState) : IRequest;
public class ChangeReportRequestStateCommandHandler : IRequestHandler<ChangeReportRequestStateCommand>
{
	private readonly MainDbContext _context;
	private readonly IWebsocketHandler _websocket;
	public ChangeReportRequestStateCommandHandler(MainDbContext context, IWebsocketHandler websocket)
	{
		_context = context;
		_websocket = websocket;
	}

	public async Task Handle(ChangeReportRequestStateCommand requestState, CancellationToken ct)
	{
		await _context.ReportRequests
			.Where(x => x.Id == requestState.ReportRequestId)
			.ExecuteUpdateAsync(calls => calls.SetProperty(p => p.State, requestState.NewState));
		await _websocket.SendReportStateAsync(requestState.ReportRequestId, requestState.NewState, ct);
	}
}