
using FluentValidation;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Validator : Validator<Request>
{
	public Validator()
	{
		RuleFor(x => x.CustomName).MaximumLength(200);

		RuleFor(x => x.Sources)
			.Must(x => x.Count > 0)
			.WithMessage("Должен быть выбран хотя-бы один источник");

		RuleForEach(x => x.Sources)
			.MinimumLength(1)
			.MaximumLength(150);
	}
}