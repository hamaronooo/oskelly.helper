
namespace KutCode.Cve.Domain.Models.CveResolver;

public class SingleCveResolveRequest : BaseCveResolveRequest
{
	public CveId CveId { get; set; }
}