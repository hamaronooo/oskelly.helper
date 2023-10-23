
namespace KutCode.Cve.Domain.Models.CveLoader;

public abstract class BaseCveLoadRequest
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
	public string LoaderCode { get; set; }
}