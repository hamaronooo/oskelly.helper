using KutCode.Cve.Application.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.CQRS.Cve;

public sealed record EditOrCreateCveFromMitreCommand(int Year, ICveLoader CveLoader) : IRequest;

public sealed class EditOrCreateCveFromMitreCommandHandler : IRequestHandler<EditOrCreateCveFromMitreCommand>
{
	private readonly MainDbContext _context;

	public EditOrCreateCveFromMitreCommandHandler(MainDbContext context)
	{
		_context = context;
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
			.NoUpdate().RunAsync(ct);
	}
}