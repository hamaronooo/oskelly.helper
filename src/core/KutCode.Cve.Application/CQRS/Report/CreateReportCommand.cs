
using KutCode.Cve.Domain.Dto.Entities.Report;
using MediatR;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record CreateReportCommand(ReportRequestDto Request) : IRequest<ReportRequestDto>;
public sealed class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportRequestDto>
{
	public async Task<ReportRequestDto> Handle(CreateReportCommand request, CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}