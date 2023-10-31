
using KutCode.Cve.Application.CQRS.Report;
using KutCode.Cve.Application.Interfaces;

namespace KutCode.Cve.Api.Endpoints.Report.Download;

public sealed class Endpoint : EndpointWithoutRequest
{
	private readonly IFileService _fileService;
	private readonly IMediator _mediator;
	public Endpoint(IFileService fileService, IMediator mediator)
	{
		_fileService = fileService;
		_mediator = mediator;
	}

	public override void Configure()
	{
		Version(1);
		AllowAnonymous();
		Get("report/download/{reportId}");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		Guid reportId = Route<Guid>("reportId");
		var reportData = await _mediator.Send(new ReportByIdQuery(reportId),ct);
		if (!reportData.HasValue) ThrowError("Отчет не найден", 404);
		if (_fileService.GetFileData(reportId).IsExist == false) ThrowError("Файл не найден", 404);

		var bytes = await _fileService.GetFileBytesAsync(reportId, ct);
		await SendBytesAsync(bytes, fileName: $"{reportData.Value!.CustomName}.xlsx");
	}
}