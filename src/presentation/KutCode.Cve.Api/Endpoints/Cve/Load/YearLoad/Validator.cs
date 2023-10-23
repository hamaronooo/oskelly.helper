using FastEndpoints;
using FluentValidation;

namespace KutCode.Cve.Api.Endpoints.Cve.Load.YearLoad;

public class Validator : Validator<Request>
{
	public Validator()
	{
		RuleFor(x => x.Year)
			.Must(x => x is > 1997 and < 2300)
			.WithMessage("Год указан некорректно");
	}
}