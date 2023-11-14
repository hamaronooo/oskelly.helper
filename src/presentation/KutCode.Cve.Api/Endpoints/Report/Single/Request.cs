using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.Single;

public sealed class Request
{
	/// <summary>
	/// Cve-Id in format CVE-XXXX-YYYYYY
	/// </summary>
	public string CveId { get; set; }
	
	public string? Software { get; set; }
	public string? Platform { get; set; }
	public ReportSearchStrategy SearchStrategy { get;set; }
	public bool IsTranslate { get; set; } = false;
	
	/// <summary>
	/// CVE-finders codes
	/// </summary>
	public List<string> Sources { get; set; }

	public string ReduceSources() => string.Join(',', Sources);
}