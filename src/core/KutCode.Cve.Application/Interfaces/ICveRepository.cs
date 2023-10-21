using System.ComponentModel;

namespace KutCode.Cve.Application.Interfaces;

public interface ICveRepository
{
	[Description("Get CVE locked status, that can display is some operations like Updates is running now on this CVE")]
	Task<bool> GetCveLockedAsync(CveId cveId, CancellationToken ct = default);
	
	[Description("Change CVE locked status")]
	Task SetCveLockedAsync(CveId cveId, bool locked = true, CancellationToken ct = default);
	
	/// <returns>Is created success</returns>
	Task UpsertCveAsync(CveDto cve, CancellationToken ct = default);
	Task UpsertCveAsync(IEnumerable<CveDto> cve, CancellationToken ct = default);
}