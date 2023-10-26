using KutCode.Cve.Application.Database;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Models.CveLoader;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KutCode.Cve.Application.CQRS.Cve;

/// <summary>
/// Загрузить CVE в локальную базу данных
/// </summary>
public sealed record LoadCveCommand(YearCveLoadRequest LoadRequest) : IRequest;
public sealed class LoadCveCommandHandler : IRequestHandler<LoadCveCommand>
{
	private readonly MainDbContext _context;
	private readonly ICveCache _cveCache;
	private readonly ICveLoaderManager _cveLoaderManager;

	public LoadCveCommandHandler(
		MainDbContext context, ICveCache cveCache, ICveLoaderManager cveLoaderManager)
	{
		_context = context;
		_cveCache = cveCache;
		_cveLoaderManager = cveLoaderManager;
	}

	public async Task Handle(LoadCveCommand request, CancellationToken ct)
	{
		var loader = _cveLoaderManager.GetLoader(request.LoadRequest.LoaderCode);
		if (loader is null) {
			Log.Error("{ClassName}; Loader with code {LCode} is not found", GetType().Name, request.LoadRequest.LoaderCode);
			return;
		}
		var cve = await loader.LoadCveByYearAsync(request.LoadRequest.Year, ct);
		await _context.Cve
			.UpsertRange(cve.Select(x => new CveEntity(x.CveId) {
				ShortName = x.ShortName,
				DescriptionEnglish = x.DescriptionEnglish,
				DescriptionRussian = x.DescriptionRussian,
				CvssMaximumRate = x.CvssMaximumRate
			}))
			.On(x => new { x.Year, x.CnaNumber })
			.NoUpdate()
			.RunAsync(ct);
		
		_cveCache.AddRange(cve.Select(x => x.CveId));
	}
}