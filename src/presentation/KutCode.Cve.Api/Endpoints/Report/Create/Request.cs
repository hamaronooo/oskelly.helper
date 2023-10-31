
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Request
{
	public string? CustomName { get;init; }

	public ReportSearchStrategy SearchStrategy { get;init; }
	
	/// <summary>
	/// CVE-finders codes
	/// </summary>
	public List<string> Sources { get; init; }

	public string ReduceSources() => string.Join(';', Sources);

	public IFormFile File { get; set; }
}