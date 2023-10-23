
namespace KutCode.Cve.Domain.Models.CveResolver;

public abstract class BaseCveResolveRequest
{
	/// <summary>
	/// Should update CVE properties
	/// </summary>
	public bool UpdateCve { get; set; }

	/// <summary>
	/// Queue priority
	/// </summary>
	public int Priority { get; set; } = 0;

	/// <summary>
	/// Cve finder code
	/// </summary>
	public string ResolverCode { get; set; }
}