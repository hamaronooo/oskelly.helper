using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.GetReportStatuses;

public sealed class Endpoint : EndpointWithoutRequest<IEnumerable<Response>>
{
	public override void Configure()
	{
		Version(1);
		AllowAnonymous();
		Get("report/statuses");
		Summary(s => s.Summary = "Жзненные статусы отчета");
	}

	public override Task<IEnumerable<Response>> ExecuteAsync(CancellationToken ct)
	{
		return Task.FromResult(Enum.GetValues<ReportRequestState>()
			.Select(x => 
				new Response((int)x, EnumHelper.GetDescriptionValue(x))));
	}
}