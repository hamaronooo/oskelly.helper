using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities.Report;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Report;

/// <summary>
/// Full report request model with CVE list
/// </summary>
public sealed record ExtendedReportByIdQuery(Guid Id) : IRequest<Optional<ReportRequestExtendedDto>>;
public sealed class ExtendedReportByIdQueryHandler : IRequestHandler<ExtendedReportByIdQuery, Optional<ReportRequestExtendedDto>>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	public ExtendedReportByIdQueryHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<Optional<ReportRequestExtendedDto>> Handle(ExtendedReportByIdQuery request, CancellationToken ct)
	{
		var response = await _context.Set<ReportRequestEntity>()
			.AsNoTracking()
			.Include(x => x.Vulnerabilities)
			.FirstOrDefaultAsync(x => x.Id == request.Id, ct);

		return _mapper.Map<ReportRequestExtendedDto>(response);
	}
}