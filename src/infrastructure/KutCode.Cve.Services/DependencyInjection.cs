using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.EntityCache;
using KutCode.Cve.Services.ResolveFounder;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services;

public static class DependencyInjection
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddHttpClient("msrc")
			.ConfigureHttpMessageHandlerBuilder(c => new HttpClientHandler
			{
				MaxConnectionsPerServer = 20
			});
		services.AddScoped<MicrosoftSecurityApiRepository>();

		services.AddSingleton<IResolveFinderManager, ResolveFinderManager>();
		services.AddScoped<MicrosoftResolveFinder>();
		services.AddSingleton<IEntityCacheService<SoftwareEntity, Guid>, SoftwareCacheService>();
		services.AddSingleton<IEntityCacheService<PlatformEntity, Guid>, PlatformCacheService>();
		return services;
	} 
}