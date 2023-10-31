
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Request
{
	public string? CustomName { get;set; }

	public ReportSearchStrategy SearchStrategy { get;set; }
	
	/// <summary>
	/// CVE-finders codes
	/// </summary>
	public List<string> Sources { get; set; }

	public string ReduceSources() => string.Join(';', Sources);

	public IFormFile File { get; set; }
}