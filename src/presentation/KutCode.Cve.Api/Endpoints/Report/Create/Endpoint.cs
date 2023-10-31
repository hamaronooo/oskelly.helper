using KutCode.Cve.Application.CQRS.Report;
using KutCode.Cve.Application.Interfaces.Excel;
using KutCode.Cve.Domain.Dto.Entities.Report;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Endpoint : Endpoint<Request,ReportRequestDto>
{
	private readonly IMediator _mediator;
	private readonly IReportRequestParser _requestParser;
	public Endpoint(IMediator mediator, IReportRequestParser requestParser)
	{
		_mediator = mediator;
		_requestParser = requestParser;
	}

	public override void Configure()
	{
		Version(1);
		AllowAnonymous();
		Post("report");
		AllowFormData();
		AllowFileUploads();
		ResponseCache(-1);
		Summary(s => {
			s.Summary = "Запрос на отчет";
			s.Description = "Необходимо прикрепить файл с CVE; Excel или CSV (';' - delimiter), где колонки соответсвуют:<br/>" +
			                "1) Cve ID (CVE-XXXX-YYYYYY)<br/>"+
			                "2) Software Name<br/>"+
			                "3) Platform Name<br/>"+
			                "4) CVE Description<br/>";
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		Log.Information("New report requst incoming");
		ThrowIfAnyErrors();
		if (Files.Count == 0) ThrowError("Нет прикрепленных файлов", 400);
		if (Files.Count > 1) ThrowError("Можно прикрепить только один файл", 400);

		using var fileStream = req.File.OpenReadStream();
		List<ReportRequestVulnerabilityPointDto> cveList = new FileInfo(req.File.FileName).Extension.ToLower() switch {
			".xlsx" => _requestParser.ParseXlsxReportRequestCve(fileStream),
			// ".csv" =>  _requestParser.ParseCsvReportRequestCve(fileStream),
			_ => Enumerable.Empty<ReportRequestVulnerabilityPointDto>().ToList()
		};
		if (cveList.Count == 0) ThrowError("Сервис не смог найти ни одного CVE для загрузки", 400);
		Log.Information("Parsed {Count} CVE for load", cveList.Count);
		var command = new ReportRequestExtendedDto() {
			CustomName = req.CustomName,
			SearchStrategy = req.SearchStrategy,
			SourcesRaw = req.ReduceSources(),
			Vulnerabilities = cveList
		};
		Log.Information("Request Sources: {Sources}", command.SourcesRaw);
		var response = await _mediator.Send(new CreateReportCommand(command), ct);
		await SendOkAsync(response, ct);
	}
}