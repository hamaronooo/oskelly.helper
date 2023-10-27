using KutCode.Cve.Application.CQRS.Report;
using MassTransit;
using Serilog;

namespace KutCode.Cve.Application.MQ;

public sealed class HandleReportRequestConsumer : IConsumer<HandleReportRequestMessage>
{
	private readonly IMediator _mediator;
	public HandleReportRequestConsumer(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Consume(ConsumeContext<HandleReportRequestMessage> context)
	{
		try {
			var result = await _mediator.Send(new HandleReportRequestCommand(context.Message.RequestId), context.CancellationToken);
		}
		catch (Exception e) {
			Log.Error(e, "{ClassName}; ERROR to handle CVE Report Request: {RequestId}", GetType().Name, context.Message.RequestId);
			throw;
		}
	}
}