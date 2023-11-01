using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Dto.Websockets;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record ChangeReportRequestStateCommand(ReportRequestStateUpdateDto Message) : IRequest;
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
			.Where(x => x.Id == requestState.Message.ReportId)
			.ExecuteUpdateAsync(calls 
				=> calls.SetProperty(p => p.State, requestState.Message.State));
		await _websocket.SendReportStateAsync(requestState.Message, ct);
	}
}