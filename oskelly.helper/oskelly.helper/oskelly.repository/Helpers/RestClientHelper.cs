using RestSharp;

namespace oskelly.repository.Helpers;

public static class RestClientHelper
{
	public static RestClient SetHeadersFromResponse(this RestClient restClient, RestResponse response)
	{
		if (response.Cookies?.Count > 0)
			restClient.AddDefaultHeaders(response.Cookies.ToDictionary(x => x.Name, z => z.Value));
		var newCookie = response.Headers?.Where(x => x.Name.ToLower() == "set-cookie").Aggregate(string.Empty, (s, parameter) => s + "; " + parameter.Value);
		restClient.AddDefaultHeader("Cookie", newCookie ?? string.Empty);
		return restClient;
	}
}