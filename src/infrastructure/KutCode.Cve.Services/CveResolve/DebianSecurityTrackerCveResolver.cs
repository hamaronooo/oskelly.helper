
using HtmlAgilityPack;
using KutCode.Cve.Application.Interfaces.Cve;

namespace KutCode.Cve.Services.CveResolve;

[CveResolver("debian_sec_track", "Debian - трекер ошибок ИБ", "security-tracker.debian.org")]
public sealed class DebianSecurityTrackerCveResolver : ICveResolver
{
	public string Code => "debian_sec_track";
	private HtmlWeb _web = new();
	public async Task<IEnumerable<VulnerabilityPointEntity>> ResolveAsync(CveId cveId, CancellationToken ct = default)
	{
		_web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36";
		HtmlDocument document = await _web.LoadFromWebAsync($"https://security-tracker.debian.org/tracker/{cveId}");
		var description = document.DocumentNode.SelectSingleNode("//table/tr/td/b[text() = 'Description']/following::td")?.InnerText;

		var notes = document.DocumentNode.SelectNodes("//h2[text() = 'Notes']/following::pre/a")
			.Select(x => x.GetAttributeValue("href", string.Empty))
			.Where(x => !string.IsNullOrEmpty(x));

		return GetProducts(document).Select(x => new VulnerabilityPointEntity {
			DataSourceCode = Code,
			CveId = cveId,
			Description = description,
			Software = new SoftwareEntity {Name = x},
			CveSolutions = new List<CveSolutionEntity> {
				new() {
					Info = x,
					AdditionalLink = notes.FirstOrDefault(),
					SolutionLink = $"https://security-tracker.debian.org/tracker/{cveId}"
				}
			}
		});
	}

	IEnumerable<string> GetProducts(HtmlDocument doc)
	{
		var allRows = doc.DocumentNode.SelectNodes("//h2[text() = 'Vulnerable and fixed packages']/following::table[2]/tr");
		if (allRows is null) return Enumerable.Empty<string>();
		var headers =
			(allRows.FirstOrDefault()?.ChildNodes.Select(x => x.InnerText).ToArray() ?? ArraySegment<string>.Empty)
			.Select((item,index) => (item,index))
			.ToList();
	
		if (headers.Count < 3) return Enumerable.Empty<string>();

		List<string> result = new();
		string currentPackage = string.Empty;
		foreach (var row in allRows.Skip(1)) {
			var cells = row.ChildNodes.Select(x => x.InnerText).ToArray();
			if (cells.Length < 1) continue;
			if (!string.IsNullOrEmpty(cells[0]))
				currentPackage = cells[0];
		
			int? versionIndex = headers.Where(x => x.item.ToLower().Contains("fixed version")).Select(x => x.index).FirstOrDefault();
			if (versionIndex is  null) continue;
			string item = $"{currentPackage} {cells[versionIndex.Value]}";

			int? releaseIndex = headers.Where(x => x.item.ToLower().Contains("release")).Select(x => x.index).FirstOrDefault();
			if (releaseIndex is not null) 
				item = $"{item} {cells[releaseIndex.Value]}";;

			result.Add(item);
		}

		return result;
	}
}