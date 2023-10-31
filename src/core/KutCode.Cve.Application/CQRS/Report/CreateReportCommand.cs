using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.MQ;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities.Report;
using KutCode.Cve.Domain.Enums;
using MassTransit;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record CreateReportCommand(ReportRequestDto Dto) : IRequest<ReportRequestDto>;
public sealed class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportRequestDto>
{
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	public CreateReportCommandHandler(MainDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
	{
		_context = context;
		_mapper = mapper;
		_publishEndpoint = publishEndpoint;
	}

	public async Task<ReportRequestDto> Handle(CreateReportCommand request, CancellationToken ct)
	{
		var entity = _mapper.Map<ReportRequestEntity>(request.Dto);
		entity.Id = Guid.NewGuid();
		entity.State = ReportRequestState.Created;
		entity.SysCreated = DateTime.Now.ToLocalTime();
		using var trans = await _context.Database.BeginTransactionAsync(ct);
		await _context.ReportRequests.AddAsync(entity, ct);
		await _context.SaveChangesAsync(ct);

		try {
			await _publishEndpoint.Publish(new HandleReportRequestMessage(entity.Id));
		}
		catch {
			await trans.RollbackAsync(ct);
			throw;
		}

		await trans.CommitAsync(ct);
		return _mapper.Map<ReportRequestDto>(entity);
	}
}