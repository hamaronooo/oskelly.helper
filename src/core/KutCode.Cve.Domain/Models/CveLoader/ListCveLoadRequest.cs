
namespace KutCode.Cve.Domain.Models.CveLoader;

/// <summary>
/// Load and create CVE in local database by CVE list
/// </summary>
public sealed class ListCveLoadRequest
{
	public List<string> CveList { get; set; }
}