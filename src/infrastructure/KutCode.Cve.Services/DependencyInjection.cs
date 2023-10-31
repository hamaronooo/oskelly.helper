using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Static;
using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.ApiRepositories.Mitre;
using KutCode.Cve.Services.CveLoad;
using KutCode.Cve.Services.CveResolve;
using KutCode.Cve.Services.CveSolution;
using KutCode.Cve.Services.EntityCache;
using KutCode.Cve.Services.File;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Services;

public static class DependencyInjection
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddHttpClient(HttpClientNames.Msrc)
			.ConfigureHttpMessageHandlerBuilder(c => new HttpClientHandler
			{
				MaxConnectionsPerServer = 20
			});
		services.AddHttpClient(HttpClientNames.Mitre)
			.ConfigureHttpMessageHandlerBuilder(c => new HttpClientHandler
			{
				MaxConnectionsPerServer = 20
			});
		services.AddScoped<MicrosoftSecurityApiRepository>();
		services.AddScoped<MitreApiRepository>();

		// resolvers
		services.AddScoped<MicrosoftCveResolver>();
		services.AddScoped<MicrosoftOldCveResolver>();
		services.AddSingleton<ICveResolverManager, CveResolverManager>();
		
		services.AddScoped<ICveResolveQueueManager, CveResolveQueueManager>();
		services.AddSingleton<ICveLoaderManager, CveLoaderManager>();
		services.AddScoped<MitreCveLoader>();
		services.AddScoped<ICveSolutionFinder, CveSolutionFinder>();
		
		services.AddSingleton<ICveCache, CveCacheService>();
		services.AddSingleton<IEntityCacheService<SoftwareEntity, Guid>, SoftwareCacheService>();
		services.AddSingleton<IEntityCacheService<PlatformEntity, Guid>, PlatformCacheService>();
		return services;
	}

	public static IServiceCollection AddFileService(this IServiceCollection services, string webRootPath)
	{
		services.AddScoped<IFileService, FileService>(x => new FileService(webRootPath));
		return services;
	}
}