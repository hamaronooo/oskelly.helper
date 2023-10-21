using KutCode.Cve.Persistence.ApiRepositories.Microsoft.Models;
using RestSharp;

namespace KutCode.Cve.Persistence.ApiRepositories.Microsoft;

public class MicrosoftSecurityApiRepository
{
    private const string CvePrefix = "cve-";
    private const string MicrosoftUrl = "https://api.msrc.microsoft.com/sug/v2.0/en-US/affectedProduct?$filter=cveNumber%20eq%20%27CVE-{cveCode}%27";
    private readonly RestClient _client;

    ~MicrosoftSecurityApiRepository()
    {
        try
        {
            _client.Dispose();
        }
        catch (Exception e)
        {
            // swallow
        }
    }
    
    public MicrosoftSecurityApiRepository()
    {
        var client = new HttpClient(new HttpClientHandler
        {
            MaxConnectionsPerServer = 100
        });
        _client = new RestClient(client);
    }

    public async Task<RestResponse<MicrosoftKbResponse>> GetCveDataAsync(string cveCode, CancellationToken ct)
    {
        var cveCodeRaw = cveCode.ToLower().Replace(CvePrefix, string.Empty).Trim();
        var request = new RestRequest(MicrosoftUrl.Replace("{cveCode}", cveCodeRaw));
        var response = await _client.ExecuteGetAsync<MicrosoftKbResponse>(request, ct);
        return response;
    }
}