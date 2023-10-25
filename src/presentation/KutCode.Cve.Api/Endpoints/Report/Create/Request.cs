
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Request
{
	public string? CustomName { get;init; }

	public ReportSearchStrategy SearchStrategy { get;init; }

	/// <summary>
	/// Resolver Code через разделитель ';'
	/// </summary>
	public string SourcesRaw { get;init; }

	public IFormFile File { get; set; }
}