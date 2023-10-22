using KutCode.Cve.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services.ResolveFounder;

public sealed class ResolveFinderManager : IResolveFinderManager
{
	private readonly IServiceScope _scope;
	public ResolveFinderManager(IServiceScopeFactory scopeFactory)
	{
		_scope = scopeFactory.CreateScope();
	}

	public IResolveFinder? GetFinder(string founderCode) => founderCode switch
	{
		"msrc" => _scope.ServiceProvider.GetRequiredService<MicrosoftResolveFinder>(),
		_ => null
	};
}