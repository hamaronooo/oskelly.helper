using FluentValidation;

namespace KutCode.Cve.Api.Endpoints.Cve.Resolve.YearResolve;

public sealed class Validator : Validator<Request>
{
	public Validator()
	{
		RuleFor(x => x.Year)
			.Must(x => x is > 1997 and < 2300)
			.WithMessage("Год указан некорректно");
	}
}