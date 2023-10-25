
using FluentValidation;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Validator : Validator<Request>
{
	public Validator()
	{
		RuleFor(x => x.CustomName).MaximumLength(200);
		RuleFor(x => x.SourcesRaw).MinimumLength(1).MaximumLength(1000);
	}
}