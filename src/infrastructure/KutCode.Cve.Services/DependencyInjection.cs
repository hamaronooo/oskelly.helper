﻿using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.CveLoad;
using KutCode.Cve.Services.CveResolve;
using KutCode.Cve.Services.EntityCache;
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

		services.AddScoped<ICveResolveQueueManager, CveResolveQueueManager>();
		services.AddSingleton<ICveResolverManager, CveResolverManager>();
		services.AddSingleton<ICveLoaderManager, CveLoaderManager>();
		services.AddScoped<MicrosoftCveResolver>();
		services.AddScoped<MitreCveLoader>();
		
		services.AddSingleton<ICveCache, CveCacheService>();
		services.AddSingleton<IEntityCacheService<SoftwareEntity, Guid>, SoftwareCacheService>();
		services.AddSingleton<IEntityCacheService<PlatformEntity, Guid>, PlatformCacheService>();
		return services;
	} 
}