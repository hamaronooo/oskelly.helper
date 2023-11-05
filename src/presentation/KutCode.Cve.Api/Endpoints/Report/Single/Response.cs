using KutCode.Cve.Domain.Dto.Entities;

namespace KutCode.Cve.Api.Endpoints.Report.Single;

public sealed class Response
{
	public bool IsSuccess { get; set; } = false;
	public string? Error { get; set; }
	public string CveId { get; set; }
	public string Platform { get; set; }
	public string Software { get; set; }
	
	/// <summary>
	/// Solutions by load source
	/// </summary>
	public Dictionary<string, List<VulnerabilityPointDto>> Solutions { get; set; }
}