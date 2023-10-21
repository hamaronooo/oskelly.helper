using MediatR;

namespace KutCode.Cve.Application.CQRS.Cve;

public sealed record EditOrCreateCveFromMitreCommand(int Year, ICveLoader CveLoader) : IRequest;

public sealed class EditOrCreateCveFromMitreCommandHandler : IRequestHandler<EditOrCreateCveFromMitreCommand>
{
	private readonly ICveRepository _cveRepository;

	public EditOrCreateCveFromMitreCommandHandler(ICveRepository cveRepository)
	{
		_cveRepository = cveRepository;
	}

	public async Task Handle(EditOrCreateCveFromMitreCommand request, CancellationToken ct)
	{
		var cve = await request.CveLoader.LoadCveByYearAsync(request.Year, ct);
		await _cveRepository.UpsertCveAsync(cve, ct);
	}
}