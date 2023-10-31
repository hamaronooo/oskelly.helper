using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.GetSearchStrategies;

public sealed class Endpoint : EndpointWithoutRequest<IEnumerable<Response>>
{
	public override void Configure()
	{
		Version(1);
		AllowAnonymous();
		Get("report/search-strategies");
		Summary(s => s.Summary = "Стратегии поиска");
	}

	public override Task<IEnumerable<Response>> ExecuteAsync(CancellationToken ct)
	{
		return Task.FromResult(Enum.GetValues<ReportSearchStrategy>()
			.Select(x => 
				new Response((int)x, EnumHelper.GetDescriptionValue(x))));
	}
}