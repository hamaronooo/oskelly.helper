using System.Xml.Linq;
using KutCode.Cve.Domain.Static;
using KutCode.Cve.Services.ApiRepositories.Mitre.Models;
using RestSharp;

namespace KutCode.Cve.Services.ApiRepositories.Mitre;

public sealed class MitreApiRepository
{
	private const string BaseUrl = "https://cveawg.mitre.org";
	private const string LoadCveXmlUrlTemplate = "https://cve.mitre.org/data/downloads/allitems-cvrf-year-{year}.xml";
	
	private readonly RestClient _client;
	public MitreApiRepository(IHttpClientFactory httpFactory)
	{
		_client = new RestClient(httpFactory.CreateClient(HttpClientNames.Mitre));
	}

	public async Task<XDocument> GetCveXmlDocumentByYearAsync(int year, CancellationToken ct = default)
	{
		var uri = LoadCveXmlUrlTemplate.Replace("{year}", year.ToString());
		RestRequest request = new RestRequest(uri) {Timeout = 120_000};
		var response = await _client.GetAsync(request, ct);
		if (response.IsSuccessStatusCode is false)
			throw new HttpRequestException(response.Content, null, response.StatusCode);
		return XDocument.Load(response.Content ?? string.Empty);
	}

	/// <summary>
	/// Load base CVE information from mitre site
	/// </summary>
	public async Task<RestResponse<MitreCveModel>> GetCveAsync(CveId cveId, CancellationToken ct = default)
	{
		var request = new RestRequest($"{BaseUrl}/api/cve/{cveId.ToString()}", Method.Get);
		return await _client.ExecuteGetAsync<MitreCveModel>(request, ct);
	}
}
