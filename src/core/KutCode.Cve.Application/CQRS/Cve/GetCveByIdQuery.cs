using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Cve;

public sealed record GetCveByIdQuery(CveId CveId) : IRequest<Optional<CveDto>>;
public class GetCveByIdQueryHandler : IRequestHandler<GetCveByIdQuery, Optional<CveDto>>
{
	private readonly ICveCache _cveCache;
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public GetCveByIdQueryHandler(ICveCache cveCache, MainDbContext context, IMapper mapper)
	{
		_cveCache = cveCache;
		_context = context;
		_mapper = mapper;
	}

	public async Task<Optional<CveDto>> Handle(GetCveByIdQuery request, CancellationToken cancellationToken)
	{
		if (_cveCache.IsExist(request.CveId) is false) return null;
		var entity =  await _context.Cve.AsNoTracking()
			.Include(x => x.Vulnerabilities)
			.ThenInclude(x => x.Software)
			.Include(x => x.Vulnerabilities)
			.ThenInclude(x => x.Platform)
			.Include(x => x.Vulnerabilities)
			.ThenInclude(x => x.CveSolutions)
			.FirstOrDefaultAsync(x => x.Year == request.CveId.Year && x.CnaNumber == request.CveId.CnaNumber, cancellationToken);
		if (entity is null) return null;
		return _mapper.Map<CveDto>(entity);
	}
}