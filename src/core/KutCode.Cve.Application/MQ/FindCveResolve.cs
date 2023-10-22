using KutCode.Cve.Application.CQRS.Cve;
using MassTransit;
using MediatR;
using Serilog;

namespace KutCode.Cve.Application.MQ;

public sealed record FindCveResolveMessage(int CveYear, string CveCnaCode, string FinderCode);

public sealed class FindCveResolveMessageConsumer : IConsumer<FindCveResolveMessage>
{
	private readonly IMediator _mediator;
	
	public FindCveResolveMessageConsumer(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Consume(ConsumeContext<FindCveResolveMessage> context)
	{
		CveId id = new CveId(context.Message.CveYear, context.Message.CveCnaCode);
		Log.Information("{ClassName}.Start finding resolves for CVE: {Cve} with source code: {SCode}", 
			GetType().Name, id, context.Message.FinderCode);
		try {
			await _mediator.Send(new FindCveResolveCommand(id, context.Message.FinderCode),
				context.CancellationToken);
		}
		catch (Exception e) {
			Log.Error(e, "{ClasName}. Error occured during handle MQ message", GetType().Name);
		}
	}
}