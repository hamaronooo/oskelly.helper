
namespace KutCode.Cve.Domain.Models.CveResolver;

/// <summary>
/// Request for CVE loading by CVE Year
/// </summary>
public class YearCveResolveRequest : BaseCveResolveRequest
{
	public int Year { get; set; }
}