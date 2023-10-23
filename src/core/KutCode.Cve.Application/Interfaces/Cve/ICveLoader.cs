using KutCode.Cve.Domain.Dto.Entities;

namespace KutCode.Cve.Application.Interfaces.Cve;

/// <summary>
/// Load raw CVE main fields and it descriptions 
/// </summary>
public interface ICveLoader
{
	public string Code { get; }
	Task<CveDto?> LoadCveAsync(CveId cveId, CancellationToken ct = default);
	Task<List<CveDto>> LoadCveByYearAsync(int year, CancellationToken ct = default);
}