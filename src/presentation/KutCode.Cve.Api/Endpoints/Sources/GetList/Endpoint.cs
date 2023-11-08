
using KutCode.Cve.Application.Interfaces.Cve;

namespace KutCode.Cve.Api.Endpoints.Sources.GetList;

public sealed class Endpoint : EndpointWithoutRequest<IEnumerable<Response>>
{
	public override void Configure()
	{
		Version(1);
		AllowAnonymous();
		Get("sources/list");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var type = typeof(ICveResolver);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => type.IsAssignableFrom(p));
		foreach (var t in types) {
			var a = t.GetProperty("Code").GetConstantValue();
		}
		await SendOkAsync(new []{}, ct);
	}
}