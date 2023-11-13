using KutCode.Cve.Application.CQRS.Report;
using KutCode.Cve.Domain.Dto.Entities;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Models;
using IMapper = AutoMapper.IMapper;

namespace KutCode.Cve.Api.Endpoints.Report.Single;

public sealed class Endpoint : Endpoint<Request, Response>
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public Endpoint(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	public override void Configure()
	{
		Version(1);
		AllowAnonymous();
		Post("report/single");
		Summary(s => {
			s.Summary = "Одиночный запрос на поиск решения CVE";
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var cveId = CveId.Parse(req.CveId);
		var command = new ReportRequestExtendedDto() {
			SearchStrategy = req.SearchStrategy,
			SourcesRaw = req.ReduceSources(),
			Vulnerabilities = new () {
				new () {
					CveYear = cveId.Year, 
					CveCnaNumber = cveId.CnaNumber,
					Software = req.Software,
					Platform = req.Platform
				}
			}
		};
		var response = new Response {
			Software = req.Software, Platform = req.Platform, CveId = cveId.ToString()
		};
		try {
			var handleResult = await _mediator.Send(new HandleSingleRequestCommand(command), ct);
			response.Solutions = handleResult.Resolves.GroupBy(x => x.Resolve.DataSourceCode)
				.ToDictionary(
					x => x.Key,
					x => x.OrderByDescending(z => z.Score)
						.Select(a => _mapper.Map<VulnerabilityPointDto>(a.Resolve))
						.ToList()
				);
			response.IsSuccess = true;
		}
		catch (Exception e) {
			response.IsSuccess = false;
			response.Error = e.Message;
		}
		await SendOkAsync(response, ct);
	}
}