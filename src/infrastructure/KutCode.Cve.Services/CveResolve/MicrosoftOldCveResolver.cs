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

		var resolves = new List<VulnerabilityPointEntity>();
		
		// main table
		{
			var tableHeaders = document.DocumentNode
				.SelectSingleNode("//p/strong[text() = 'Affected Software']/following::table[1]/thead/tr")
				.ChildNodes.Where(x => x.Name == "th").Select(x => x.InnerText).ToArray();
			var tableBody =
				document.DocumentNode.SelectSingleNode(
					"//p/strong[text() = 'Affected Software']/following::table[1]/tbody");

			if (tableBody is not null)
				resolves.AddRange(ParseTable(refUri, cveId, mitre, tableBody, tableHeaders));
		}
		// ms office table
		{
			var tableHeaders = document.DocumentNode
				.SelectSingleNode("//p/strong[text() = 'Microsoft Office']/following::table[1]/thead/tr")
				.ChildNodes.Where(x => x.Name == "th").Select(x => x.InnerText).ToArray();
			var tableBody =
				document.DocumentNode.SelectSingleNode(
					"//p/strong[text() = 'Microsoft Office']/following::table[1]/tbody");

			if (tableBody is not null)
				resolves.AddRange(ParseTable(refUri, cveId, mitre, tableBody, tableHeaders));
		}

		return resolves;
	}

	private List<VulnerabilityPointEntity> ParseTable(Uri refUri, CveId cveId, MitreCveModel mitre, HtmlNode tableBody, string[] tableHeaders)
	{
		List<VulnerabilityPointEntity> resolves = new();
		var description = mitre.Containers.Cna.Descriptions.FirstOrDefault()?.Value;
		foreach (var row in tableBody.ChildNodes.Where(x => x.Name == "tr"))
		{
			var cells = row.ChildNodes.Where(x => x.Name == "td").ToArray();
			if (cells.Length < 4) continue;

			VulnerabilityPointEntity resolve = new();

			// solutions search
			var fallbackSearch = string.Join(' ', cells.Select(c => c.InnerText));
			resolve.CveSolutions = ParseResolves(cells, tableHeaders, fallbackSearch).Select(x =>
			{
				x.SolutionLink = refUri.AbsoluteUri;
				return x;
			}).ToList();

			if (resolve.CveSolutions.Count == 0) continue;

			resolve.Platform = ParsePlatform(cells, tableHeaders);
			resolve.Software = ParseSoftware(cells, tableHeaders);
			resolve.Impact = ParseImpact(cells, tableHeaders);

			resolve.CveId = cveId;
			resolve.DataSourceCode = this.Code;
			resolve.Description = description;
			resolves.Add(resolve);
		}

		return resolves;
	}

	SoftwareEntity? ParseSoftware(HtmlNode[] cells, string[] tableHeaders)
	{
		var res = SearchByHeaderPrompt(cells, tableHeaders, "component") 
		          ?? SearchByHeaderPrompt(cells, tableHeaders, "software");
		return string.IsNullOrEmpty(res) ? null : new () { Name = res };
	}
	PlatformEntity? ParsePlatform(HtmlNode[] cells, string[] tableHeaders)
	{
		var res = SearchByHeaderPrompt(cells, tableHeaders, "operating system") 
		          ?? SearchByHeaderPrompt(cells, tableHeaders, "platform");
		return string.IsNullOrEmpty(res) ? null : new () { Name = res };
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

	IEnumerable<CveSolutionEntity> ParseResolves(HtmlNode[] cells, string[] tableHeaders, string fallbackSearch)
	{
		List<string> results = new();
		var header = tableHeaders.Select((x, index) => (x, index)).FirstOrDefault(t => t.x.ToLower().Contains("updates replaced"));
		if (cells.Length < header.index + 1)
			return Regexes.KbRegex.Matches(fallbackSearch).Select(x => x.Value).Distinct()
				.Select(x => new CveSolutionEntity {
					Info = x
				});
		
		results = Regexes.KbRegex.Matches(cells[header.index].InnerText).Select(x => x.Value).ToList();
		if (results.Count==0)
			results = Regexes.KbRegex.Matches(fallbackSearch)
				.Select(x => x.Value)
				.Distinct()
				.ToList();
		
		return results.Select(x => new CveSolutionEntity {
			Info = x
		});
	}

	private string? SearchByHeaderPrompt(HtmlNode[] cells, string[] tableHeaders, string headerPrompt)
	{
		var header = tableHeaders.Select((x, index) => (x, index)).FirstOrDefault(t => t.x.ToLower().Contains(headerPrompt.ToLower()));
		if (cells.Length < header.index + 1 || string.IsNullOrEmpty(header.x))
			return null;
		var link = cells[header.index].ChildNodes.FirstOrDefault(x => x.Name == "a");
		if (link is not null) return link.InnerText;
		return cells[header.index].InnerText;
	}
}