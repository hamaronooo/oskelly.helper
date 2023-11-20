using KutCode.Cve.Domain.Static;
using KutCode.Cve.Services.ApiRepositories.Microsoft.Models;
using RestSharp;

namespace KutCode.Cve.Services.ApiRepositories.Microsoft;

public sealed class MicrosoftSecurityApiRepository
{
    private const string NewCveUrlTemplate = "https://api.msrc.microsoft.com/sug/v2.0/en-US/affectedProduct?$filter=cveNumber%20eq%20%27CVE-{cveCode}%27";
    private readonly RestClient _client;
    
    public MicrosoftSecurityApiRepository(IHttpClientFactory httpFactory)
    {
        _client = new RestClient(httpFactory.CreateClient(HttpClientNames.Msrc));
    }

    public async Task<RestResponse<MicrosoftKbResponse>> GetCveDataAsync(CveId cveId, CancellationToken ct)
    {
        var request = new RestRequest(NewCveUrlTemplate.Replace("{cveCode}", cveId.AsStringWithoutPrefix));
        return await _client.ExecuteGetAsync<MicrosoftKbResponse>(request, ct);
    }
}