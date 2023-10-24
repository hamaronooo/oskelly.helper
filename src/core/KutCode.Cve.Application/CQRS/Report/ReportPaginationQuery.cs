
using AutoMapper;
using KutCode.Cve.Application.Database;
using KutCode.Cve.Domain.Dto.Entities.Report;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Report;

public sealed record ReportPaginationQuery(PaginationRequest Request) : IRequest<PaginationResponse<ReportRequestDto>>;

public sealed class ReportPaginationQueryHandler : IRequestHandler<ReportPaginationQuery, PaginationResponse<ReportRequestDto>>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	public ReportPaginationQueryHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<PaginationResponse<ReportRequestDto>> Handle(ReportPaginationQuery request, CancellationToken ct)
	{
		var items = await _context.ReportRequests.AsNoTracking()
			.OrderByDescending(x => x.SysCreated)
			.Skip((request.Request.Page - 1) * request.Request.OnPage)
			.Take(request.Request.OnPage)
			.Select(x => _mapper.Map<ReportRequestDto>(x))
			.ToListAsync(ct);
		return new PaginationResponse<ReportRequestDto>(request.Request, items);
	}
}