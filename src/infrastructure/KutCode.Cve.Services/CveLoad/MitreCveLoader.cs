using KutCode.Cve.Application.Interfaces.Cve;
using KutCode.Cve.Domain.Dto.Entities;
using KutCode.Cve.Services.ApiRepositories.Mitre;

namespace KutCode.Cve.Services.CveLoad;

public class MitreCveLoader : ICveLoader
{
	public string Code => "mitre";
	private readonly MitreApiRepository _mitre;
	public MitreCveLoader(MitreApiRepository mitre)
	{
		_mitre = mitre;
	}

	public async Task<CveDto?> LoadCveAsync(CveId cveId, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}

	public async Task<List<CveDto>> LoadCveByYearAsync(int year, CancellationToken ct = default)
	{
		var xmlDoc = await _mitre.GetCveXmlDocumentByYearAsync(year, ct);
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
			response.Add(new CveDto {
				CveId = cveId, DescriptionEnglish = rawDescription
			});
		}
		return response;
	}

	
}