
namespace KutCode.Cve.Domain.Models.CveResolver;

/// <summary>
/// Request for CVE loading by presented CVE list
/// </summary>
public class ListCveResolveRequest : BaseCveResolveRequest
{
	public List<string> CveList { get; set; }
}