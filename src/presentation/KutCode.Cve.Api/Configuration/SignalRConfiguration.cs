
using KutCode.Cve.Application.Hubs;

namespace KutCode.Cve.Api.Configuration;

public static class SignalRConfiguration
{
	public static WebApplicationBuilder ConfigureSignalR(this WebApplicationBuilder builder)
	{
		builder.Services.AddSignalR(opts => {
			// options 
		});
		return builder;
	}
	
	public static WebApplication UseSignalR(this WebApplication app)
	{
		app.MapHub<ReportHub>("/ws/report");

		return app;
	}
}
