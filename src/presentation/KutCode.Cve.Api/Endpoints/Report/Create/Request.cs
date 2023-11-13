
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Request
{
	public string? CustomName { get;set; }

	public ReportSearchStrategy SearchStrategy { get;set; }

	/// <summary>
	/// Сортировать вывод по CVE ID
	/// </summary>
	public bool IsReorder { get; set; } = false;
	
	/// <summary>
	/// CVE-finders codes
	/// </summary>
	public List<string> Sources { get; set; }

	public string ReduceSources() => string.Join(',', Sources);

	public IFormFile File { get; set; }
}