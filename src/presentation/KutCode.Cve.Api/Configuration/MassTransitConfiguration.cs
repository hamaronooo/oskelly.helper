using KutCode.Cve.Application.MQ;
using MassTransit;

namespace KutCode.Cve.Api.Configuration;

public static class MassTransitConfiguration
{
	public static WebApplicationBuilder AddMassTransitConfiguration(this WebApplicationBuilder builder)
	{
		builder.Services.AddOptions<RabbitMqTransportOptions>().Configure(options => {
			builder.Configuration.Bind("Rabbit", options);
		});

		builder.Services.AddMassTransit(x => {
			x.AddConsumer<HandleReportRequestConsumer>(x => {
				x.UseMessageRetry(r => r.Exponential(3, TimeSpan.FromSeconds(60), TimeSpan.FromMinutes(10),
						TimeSpan.FromSeconds(60))
				);
			});
			x.UsingRabbitMq((context, cfg) => {
				cfg.Durable = true;
				cfg.ConfigureEndpoints(context);
			});
		});

		return builder;
	}
}