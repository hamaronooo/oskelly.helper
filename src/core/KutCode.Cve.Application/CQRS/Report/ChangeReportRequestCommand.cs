using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Entities.Report;
using KutCode.Cve.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record ChangeReportRequestCommand(Guid ReportRequestId, ReportRequestState NewState) : IRequest;
public class ChangeReportRequestCommandHandler : IRequestHandler<ChangeReportRequestCommand>
{
	private readonly MainDbContext _context;

	public ChangeReportRequestCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public async Task Handle(ChangeReportRequestCommand request, CancellationToken cancellationToken)
	{
		await _context.ReportRequests
			.Where(x => x.Id == request.ReportRequestId)
			.ExecuteUpdateAsync(calls => calls.SetProperty(p => p.State, request.NewState));
	}
}