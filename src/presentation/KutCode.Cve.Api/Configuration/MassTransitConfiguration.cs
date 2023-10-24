using MassTransit;

namespace KutCode.Cve.Api.Configuration;

public static class MassTransitConfiguration
{
	public static WebApplicationBuilder AddMassTransitConfiguration(this WebApplicationBuilder builder)
	{
		builder.Services.AddOptions<RabbitMqTransportOptions>().Configure(options => {
			builder.Configuration.Bind("Rabbit", options);
		});

		// builder.Services.AddMassTransit(x => {
		// 	x.AddConsumer<FindCveResolveMessageConsumer>();
		// 	x.UsingRabbitMq((context, cfg) => {
		// 		cfg.Durable = true;
		// 		cfg.ConfigureEndpoints(context);
		// 	});
		// });

		return builder;
	}
}