
using KutCode.Cve.Application.Interfaces.Cve;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services.CveLoad;

public sealed class CveLoaderManager : ICveLoaderManager
{
	private readonly IServiceScope _scope;
	public CveLoaderManager(IServiceScopeFactory scopeFactory)
	{
		_scope = scopeFactory.CreateScope();
	}

	public ICveLoader? GetLoader(string code) => code switch
	{
		"mitre" => _scope.ServiceProvider.GetRequiredService<MitreCveLoader>(),
		_ => null
	};
}