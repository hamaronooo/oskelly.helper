using FluentValidation;
using KutCode.Cve.Domain.Models;

namespace KutCode.Cve.Api.Endpoints.Report.Single;

public sealed class Validator : Validator<Request>
{
	public Validator()
	{
		RuleFor(x => x.Software)
			.MaximumLength(500);
		
		RuleFor(x => x.Platform)
			.MaximumLength(500);

		RuleFor(x => x.CveId)
			.Must(x => CveId.TryParse(x, out _))
			.WithMessage("Id CVE имеет некорректный формат");

		RuleFor(x => x.Sources)
			.Must(x => x.Count > 0)
			.WithMessage("Хотя бы один Источник Поиска должен быть указан");
	}
}