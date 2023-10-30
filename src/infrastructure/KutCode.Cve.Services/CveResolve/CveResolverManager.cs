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

	public Optional<ICveResolver> GetResolver(string resolverCode) => resolverCode switch
	{
		"msrc" => _scope.ServiceProvider.GetRequiredService<MicrosoftCveResolver>(),
		"msrc_old" => _scope.ServiceProvider.GetRequiredService<MicrosoftOldCveResolver>(),
		_ => Optional<ICveResolver>.None
	};
}