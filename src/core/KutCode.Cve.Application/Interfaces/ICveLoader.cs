using KutCode.Cve.Domain.Dto.Entities;

namespace KutCode.Cve.Application.Interfaces;

/// <summary>
/// Load raw CVE main fields and it descriptions 
/// </summary>
public interface ICveLoader
{
	Task<CveDto?> LoadCveAsync(CveId cveId, CancellationToken ct = default);
	Task<List<CveDto>> LoadCveByYearAsync(int year, CancellationToken ct = default);
}