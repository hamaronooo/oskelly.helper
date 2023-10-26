using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using MediatR;

namespace KutCode.Cve.Application.CQRS.Cve;

/// <summary>
/// Обновить рейтинг угрозы CVE
/// </summary>
public sealed record UpdateCveCvssCommand(CveId CveId, double Cvss) : IRequest;
public sealed class UpdateCveCvssCommandHandler : IRequestHandler<UpdateCveCvssCommand>
{
	private readonly MainDbContext _context;
	private readonly ICveCache _cveCache;

	public UpdateCveCvssCommandHandler(MainDbContext context, ICveCache cveCache)
	{
		_context = context;
		_cveCache = cveCache;
	}

	public async Task Handle(UpdateCveCvssCommand request, CancellationToken cancellationToken)
	{
		if (_cveCache.IsExist(request.CveId) is false) return;
		_context.Cve.Attach(new CveEntity(request.CveId) { CvssMaximumRate = request.Cvss })
			.Property(x => x.CvssMaximumRate).IsModified = true;
		await _context.SaveChangesAsync(cancellationToken);
	}
}