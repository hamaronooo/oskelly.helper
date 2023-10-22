using KutCode.Cve.Application.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Cve;

public sealed record EditOrCreateCveFromMitreCommand(int Year, ICveLoader CveLoader) : IRequest;

public sealed class EditOrCreateCveFromMitreCommandHandler : IRequestHandler<EditOrCreateCveFromMitreCommand>
{
	private readonly MainDbContext _context;
	private readonly ICveCache _cveCache;

	public EditOrCreateCveFromMitreCommandHandler(MainDbContext context, ICveCache cveCache)
	{
		_context = context;
		_cveCache = cveCache;
	}

	public async Task Handle(EditOrCreateCveFromMitreCommand request, CancellationToken ct)
	{
		var cve = await request.CveLoader.LoadCveByYearAsync(request.Year, ct);
		await _context.Cve
			.UpsertRange(cve.Select(x => new CveEntity(x.CveId) {
				ShortName = x.ShortName,
				DescriptionEnglish = x.DescriptionEnglish,
				DescriptionRussian = x.DescriptionRussian,
				CvssMaximumRate = x.CvssMaximumRate
			}))
			.On(x => new { x.Year, x.CnaNumber })
			.NoUpdate()
			.RunAsync(ct);
		
		_cveCache.AddRange(cve.Select(x => x.CveId));
	}
}