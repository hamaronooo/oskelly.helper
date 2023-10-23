using System.Xml.Linq;
using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities;

namespace KutCode.Cve.Services.CveLoad;

public class MitreCveLoader : ICveLoader
{
	private const string Url = "https://cve.mitre.org/data/downloads/allitems-cvrf-year-{year}.xml";

	public string Code => "mitre";

	public async Task<CveDto?> LoadCveAsync(CveId cveId, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}

	public async Task<List<CveDto>> LoadCveByYearAsync(int year, CancellationToken ct = default)
	{
		var xmlDoc = await GetXmlDocumentAsync(year, ct);
		List<CveDto> response = new(1000);
		var vuls = xmlDoc.Descendants().Where(x => x.Name.LocalName == "Vulnerability");
		foreach (var vul in vuls)
		{
			var rawCveId = vul.Descendants().FirstOrDefault(x => x.Name.LocalName == "Title")?.Value;
			if (string.IsNullOrEmpty(rawCveId)) continue;
			CveId cveId = CveId.Parse(rawCveId);
			var rawDescription = vul.Descendants()
				.FirstOrDefault(x =>x.Name.LocalName == "Note" && x.Attribute("Type")?.Value.ToLower() == "description")?.Value;
			if (string.IsNullOrEmpty(rawDescription)) continue;
			response.Add(new CveDto
			{
				CveId = cveId, DescriptionEnglish = rawDescription
			});
		}
		return response;
	}

	private async Task<XDocument> GetXmlDocumentAsync(int year, CancellationToken ct = default)
	{
		var client = new HttpClient();
		client.Timeout = TimeSpan.FromSeconds(120);
		var uri = Url.Replace("{year}", year.ToString());
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
		using var response = await client.SendAsync(request, ct);
		if (response.IsSuccessStatusCode is false)
			throw new HttpRequestException(await response.Content.ReadAsStringAsync(ct), null, response.StatusCode);
		await using var contentStream = await response.Content.ReadAsStreamAsync(ct);
		return XDocument.Load(contentStream);
	}
}