using KutCode.Cve.Application.Interfaces;
using KutCode.Cve.Domain.Dto;
using KutCode.Cve.Domain.Entities;
using KutCode.Cve.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Persistence.Database;

public sealed class CveRepository : ICveRepository
{
	private readonly MainDbContext _context;

	public CveRepository(MainDbContext context)
	{
		_context = context;
	}

	public async Task<bool> GetCveLockedAsync(CveId cveId, CancellationToken ct = default)
		=> await _context.Cve.AsNoTracking()
			.Where(x => x.Year == cveId.Year && x.CnaNumber == cveId.CnaNumber)
			.Select(x => x.Locked).FirstOrDefaultAsync(ct);

	public async Task SetCveLockedAsync(CveId cveId, bool locked = true, CancellationToken ct = default)
	{
		_context.Cve.Entry(new CveEntity(cveId) {Locked = locked}).Property(x => x.Locked).IsModified = true;
		await _context.SaveChangesAsync(ct);
	}
	
	public async Task UpsertCveAsync(CveDto cve, CancellationToken ct = default)
	{
		await _context.Cve
			.Upsert(new CveEntity(cve.CveId) {
				ShortName = cve.ShortName, DescriptionEnglish = cve.Description, CVSS = cve.CVSS
			})
			.On(x => new { x.Year, x.CnaNumber })
			.NoUpdate().RunAsync(ct);
	}

	public async Task UpsertCveAsync(IEnumerable<CveDto> cve, CancellationToken ct = default)
	{
		await _context.Cve
			.UpsertRange(cve.Select(x => new CveEntity(x.CveId) {
				ShortName = x.ShortName, DescriptionEnglish = x.Description, CVSS = x.CVSS
			}))
			.On(x => new { x.Year, x.CnaNumber })
			.NoUpdate().RunAsync(ct);
	}

	
}