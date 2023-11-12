using HtmlAgilityPack;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain;
using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.ApiRepositories.Mitre;
using KutCode.Cve.Services.ApiRepositories.Mitre.Models;

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
				var results = await LoadAsync(refUri, cveId, mitreCve.Data);
				result.AddRange(results);
			}
		}
		return result;
	}

	private async Task<IEnumerable<VulnerabilityPointEntity>> LoadAsync(Uri refUri, CveId cveId, MitreCveModel mitre)
	{
		HtmlWeb web = new();
		HtmlDocument document = await web.LoadFromWebAsync(refUri.AbsoluteUri);
		var tableHeaders = document.DocumentNode
			.SelectSingleNode("//p/strong[text() = 'Affected Software']/following::table[1]/thead/tr")
			.ChildNodes.Where(x => x.Name == "th").Select(x => x.InnerText).ToArray();
		var tableBody = document.DocumentNode.SelectSingleNode("//p/strong[text() = 'Affected Software']/following::table[1]/tbody");
		
		if (tableBody is null) return Enumerable.Empty<VulnerabilityPointEntity>();

		List<VulnerabilityPointEntity> resolves = new();
		var description = mitre.Containers.Cna.Descriptions.FirstOrDefault()?.Value;
		foreach (var row in tableBody.ChildNodes.Where(x => x.Name == "tr"))
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
						resolve.Impact = ParseImpact(cells, tableHeaders);
						break;
					case 3:
						var fallbackSearch = string.Join(' ', cells.Select(c => c.InnerText));
						resolve.CveSolutions = ParseResolves(cells[i], fallbackSearch).Select(x => {
							x.SolutionLink = refUri.AbsoluteUri;
							return x;
						}).ToList();
						break;
				}
			}
			if (string.IsNullOrEmpty(resolve.Platform?.Name) || resolve.CveSolutions.Count == 0) continue;
			resolve.CveId = cveId;
			resolve.DataSourceCode = this.Code;
			resolve.Description = description;
			resolves.Add(resolve);
		}

		return resolves;
	}

	string ParseImpact(HtmlNode[] cells, string[] tableHeaders)
	{
		var header = tableHeaders.Select((x, index) => (x, index)).FirstOrDefault(t => t.x.ToLower().Contains("impact"));
		if (cells.Length < header.index + 1)
			return cells[1].InnerText.Normalize();
		var link = cells[header.index].ChildNodes.FirstOrDefault(x => x.Name == "a");
		if (link is not null) return link.InnerText;
		return cells[header.index].InnerText;
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

	IEnumerable<CveSolutionEntity> ParseResolves(HtmlNode cell, string fallbackSearch)
	{
		var result = Regexes.KbRegex.Matches(cell.InnerText).Select(x => x.Value).ToList();
		if (result.Count==0)
			result = Regexes.KbRegex.Matches(fallbackSearch)
				.Select(x => x.Value)
				.Distinct()
				.ToList();
		return result.Select(x => new CveSolutionEntity {
			Info = x
		});
	}
}