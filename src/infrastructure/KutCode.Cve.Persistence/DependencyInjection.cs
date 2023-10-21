using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Cve.Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddMainDbContext(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<MainDbContext>(x =>
		{
			x.UseNpgsql(connectionString);
		});
		return services;
	}

	// public static IServiceCollection AddMainDapperRepo(this IServiceCollection services, string connectionString)
	// {
	// 	services.AddScoped<MainDapperRepository>(x => new MainDapperRepository(connectionString));
	// 	return services;
	// }
	// public static IServiceCollection AddMsSecurityApiRepo(this IServiceCollection services)
	// {
	// 	services.AddScoped<MicrosoftSecurityApiRepository>();
	// 	return services;
	// }
}