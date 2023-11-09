using System.Reflection;
using KutCode.Cve.Application.Interfaces.Cve;

namespace KutCode.Cve.Api.Endpoints.Sources.GetList;

public sealed class Endpoint : EndpointWithoutRequest<IEnumerable<CveResolverListItem>>
{
	public ICveResolverManager ResolverManager { get; set; }
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
		List<CveResolverListItem> result = new();
		foreach (var t in types) {
			Attribute? attributeRaw = t.GetCustomAttribute(typeof(CveResolverAttribute));
			if (attributeRaw is null) continue;
			var attribute = (CveResolverAttribute) attributeRaw;
			result.Add(new (attribute.Code, attribute.Name, attribute.Domain, attribute.Enabled));
		}
		await SendOkAsync(result, ct);
	}
}