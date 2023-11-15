using System.Reflection;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Optionality;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services.CveResolve;

public sealed class CveResolverManager : ICveResolverManager
{
	private readonly IServiceScope _scope;
	public CveResolverManager(IServiceScopeFactory scopeFactory)
	{
		_scope = scopeFactory.CreateScope();
	}

	public Optional<ICveResolver> GetResolver(string resolverCode)
	{
		foreach (var type in ResolverTypes)
		{
			var attributeData = GetResolverAttribute(type);
			if (attributeData.HasValue is false) continue;
			if (attributeData.Value!.Code.Equals(resolverCode.Trim()))
			{
				var providerResult = _scope.ServiceProvider.GetService(type);
				if (providerResult is null || providerResult is not ICveResolver resolver)
					continue;
				return Optional.From(resolver);
			} 
		}
		return Optional.None<ICveResolver>();
	}

	public IEnumerable<CveResolverListItem> GetResolversData()
	{
		foreach (var type in ResolverTypes) {
			Attribute? attributeRaw = type.GetCustomAttribute(typeof(CveResolverAttribute));
			if (attributeRaw is null) continue;
			var attribute = (CveResolverAttribute) attributeRaw;
			yield return new(attribute.Code, attribute.Name, attribute.Domain, attribute.Enabled);
		}
	}

	public static IEnumerable<Type> ResolverTypes => AppDomain.CurrentDomain.GetAssemblies()
		.SelectMany(ass => ass.GetTypes())
		.Where(t => typeof(ICveResolver).IsAssignableFrom(t))
		.Where(t => t.IsInterface is false && t.IsAbstract is false);
	
	private static Optional<CveResolverAttribute> GetResolverAttribute(Type resolverType)
	{
		Attribute? attributeRaw = resolverType.GetCustomAttribute(typeof(CveResolverAttribute));
		if (attributeRaw is null || attributeRaw is not CveResolverAttribute result)
			return Optional.None<CveResolverAttribute>();
		return Optional.From(result);
	}
}