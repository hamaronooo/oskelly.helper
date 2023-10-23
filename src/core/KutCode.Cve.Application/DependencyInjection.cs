using KutCode.Cve.Application.CveFinder;
using KutCode.Cve.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddMainDbContext(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<MainDbContext>(x => {
			x.UseNpgsql(connectionString);
		});
		return services;
	}
	
	public static IServiceCollection AddCveFinderProcessor(this IServiceCollection services)
	{
		services.AddSingleton<CveResolveProcessor>();
		return services;
	}
}