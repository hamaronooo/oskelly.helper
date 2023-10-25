using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities.Report;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Report;

/// <summary>
/// Light report request model withOut CVE list
/// </summary>
public sealed record ReportByIdQuery(Guid Id) : IRequest<Optional<ReportRequestDto>>;
public sealed class ReportByIdQueryHandler : IRequestHandler<ReportByIdQuery, Optional<ReportRequestDto>>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	public ReportByIdQueryHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<Optional<ReportRequestDto>> Handle(ReportByIdQuery request, CancellationToken ct)
	{
		var response = await _context.Set<ReportRequestEntity>()
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.Id, ct);

		return _mapper.Map<ReportRequestDto>(response);
	}
}