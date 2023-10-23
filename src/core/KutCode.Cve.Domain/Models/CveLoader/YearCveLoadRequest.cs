
namespace KutCode.Cve.Domain.Models.CveLoader;

/// <summary>
/// Load and create CVE in local database by Year
/// </summary>
public class YearCveLoadRequest : BaseCveLoadRequest
{
	public int Year { get; set; }
}