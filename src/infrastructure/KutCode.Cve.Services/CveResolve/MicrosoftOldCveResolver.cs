using HtmlAgilityPack;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain;
using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.ApiRepositories.Mitre;

namespace KutCode.Cve.Services.CveResolve;

/// <summary>
/// Resolve CVE from MSRC site from old Microsoft vulnerability system (non CVE)
/// </summary>
[CveResolver("msrc_old", "Устаревший репозиторий Microsoft (до 2018)", "learn.microsoft.com")]
public sealed class MicrosoftOldCveResolver : ICveResolver
{
	private readonly MicrosoftSecurityApiRepository _msrcApi;
	private readonly MitreApiRepository _mitreApi;

	public MicrosoftOldCveResolver(
		MicrosoftSecurityApiRepository msrcApi,
		MitreApiRepository mitreApi)
	{
		_msrcApi = msrcApi;
		_mitreApi = mitreApi;
	}

	public string Code => "msrc_old";
	public Uri Uri => new("https://learn.microsoft.com/");

	// todo: finish her
	public async Task<IEnumerable<VulnerabilityPointEntity>> ResolveAsync(CveId cveId, CancellationToken ct = default)
	{
		var mitreCve = await _mitreApi.GetCveAsync(cveId, ct);
		if (mitreCve.Data == null || mitreCve.IsSuccessful == false) return Enumerable.Empty<VulnerabilityPointEntity>();

		var result = new List<VulnerabilityPointEntity>();
		foreach (var reference in mitreCve.Data.Containers.Cna.References) {
			Uri refUri = new(reference.Url);
			if (refUri.Host == "learn.microsoft.com" || refUri.Host == "docs.microsoft.com") {
				// goto link, download xml page and try parse it
				// try get Affected Software block table
				var results = await LoadAsync(refUri);
				result.AddRange(results);
			}
		}
		return result;
	}

	private async Task<IEnumerable<VulnerabilityPointEntity>> LoadAsync(Uri refUri)
	{
		HtmlWeb web = new();
		HtmlDocument document = await web.LoadFromWebAsync(refUri.AbsoluteUri);
		var table = document.DocumentNode
			.SelectSingleNode("//p/strong[text() = 'Affected Software']/following::table[1]/tbody");
		
		if (table is null) return Enumerable.Empty<VulnerabilityPointEntity>();

		List<VulnerabilityPointEntity> resolves = new();
		foreach (var row in table.ChildNodes.Where(x => x.Name == "tr"))
		{
			var cells = row.ChildNodes.Where(x => x.Name == "td").ToArray();
			if (cells.Length < 4) continue;
			VulnerabilityPointEntity resolve = new();
			for (int i = 0; i < cells.Length; i++)
			{
				var text = cells[i].InnerText;
				if (string.IsNullOrEmpty(text)) continue;
				switch (i)
				{
					case 0:
						resolve.Platform = ParseProductName(cells[i]);
						resolve.Software = new SoftwareEntity { Name = resolve.Platform.Name };
						break;
					case 1:
						resolve.Impact = ParseImpact(cells[i]);
						break;
					case 3:
						resolve.CveSolutions = ParseResolves(cells[i], cells[0]).ToList();
						break;
				}
			}
			if (string.IsNullOrEmpty(resolve.Platform?.Name) || resolve.CveSolutions.Count == 0) continue;
			resolves.Add(resolve);
		}

		return resolves;
	}
	
	PlatformEntity ParseProductName(HtmlNode cell)
	{
		var nameLink = cell.ChildNodes.FirstOrDefault(x => x.Name == "a");
		if (nameLink is not null)
			return new() { Name = nameLink.InnerText };
		return new() {
			Name = Regexes.KbRegex.Replace(cell.InnerText, string.Empty)
		};
	}
	string ParseImpact(HtmlNode cell)
	{
		return cell.InnerText.Normalize();
	}
	IEnumerable<CveSolutionEntity> ParseResolves(HtmlNode cell, HtmlNode nameCell)
	{
		var result = Regexes.KbRegex.Matches(cell.InnerText).Select(x => x.Value).ToList();
		if (result.Count==0)
			result = Regexes.KbRegex.Matches(nameCell.InnerText).Select(x => x.Value).ToList();
		return result.Select(x => new CveSolutionEntity {
			Info = x
		});
	}
}