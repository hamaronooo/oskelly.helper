using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities.Report;
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record CreateReportCommand(ReportRequestDto Dto) : IRequest<ReportRequestDto>;
public sealed class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportRequestDto>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	public CreateReportCommandHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ReportRequestDto> Handle(CreateReportCommand request, CancellationToken ct)
	{
		var entity = _mapper.Map<ReportRequestEntity>(request.Dto);
		entity.Id = Guid.NewGuid();
		entity.State = ReportRequestState.Created;
		entity.SysCreated = DateTime.Now.ToLocalTime();
		await _context.ReportRequests.AddAsync(entity, ct);
		await _context.SaveChangesAsync(ct);
		return _mapper.Map<ReportRequestDto>(entity);
	}
}