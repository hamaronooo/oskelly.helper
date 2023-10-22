using KutCode.Cve.Services.ApiRepositories.Microsoft.Models;
using RestSharp;

namespace KutCode.Cve.Services.ApiRepositories.Microsoft;

public sealed class MicrosoftSecurityApiRepository
{
    private const string MicrosoftUrl = "https://api.msrc.microsoft.com/sug/v2.0/en-US/affectedProduct?$filter=cveNumber%20eq%20%27CVE-{cveCode}%27";
    private readonly RestClient _client;
    
    public MicrosoftSecurityApiRepository(IHttpClientFactory httpFactory)
    {
        _client = new RestClient(httpFactory.CreateClient("msrc"));
    }

    public async Task<RestResponse<MicrosoftKbResponse>> GetCveDataAsync(CveId cveId, CancellationToken ct)
    {
        var request = new RestRequest(MicrosoftUrl.Replace("{cveCode}", cveId.AsStringWithoutPrefix));
        var response = await _client.ExecuteGetAsync<MicrosoftKbResponse>(request, ct);
        return response;
    }
}