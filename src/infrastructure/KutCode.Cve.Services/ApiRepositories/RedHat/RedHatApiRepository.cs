
using KutCode.Cve.Domain.Static;
using KutCode.Cve.Services.ApiRepositories.RedHat.Models;
using RestSharp;

namespace KutCode.Cve.Services.ApiRepositories.RedHat;

public sealed class RedHatApiRepository
{
	private const string CveUrlTemplate = "https://access.redhat.com/security/cve/{cveid}";
	private readonly RestClient _client;
    
	public RedHatApiRepository(IHttpClientFactory httpFactory)
	{
		_client = new RestClient(httpFactory.CreateClient(HttpClientNames.RedHat));
	}

	public async Task<RestResponse<RedhatCveJsonModel>> GetCveAsync(CveId cveId, CancellationToken ct = default)
		=> await _client.ExecuteGetAsync<RedhatCveJsonModel>(
			new RestRequest(CveUrlTemplate.Replace("{cveid}", cveId.AsString))
		);
	
	
}