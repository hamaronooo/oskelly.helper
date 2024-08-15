using Newtonsoft.Json;
using oskelly.helper.OskellyRespository.Models;
using oskelly.helper.OskellyRespository.Models.Catalog;
using oskelly.helper.OskellyRespository.Models.ProductData;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

public class OskellyRepository
{
	private readonly RestClient _client;
	
	public OskellyRepository()
	{
		_client = new RestClient(new RestClientOptions("https://oskelly.ru"),
			configureDefaultHeaders: headers => {
				headers.Add("sec-ch-ua", """Not)A;Brand";v="99", "Google Chrome";v="127", "Chromium";v="127""");
				headers.Add("sec-ch-ua-mobile", "?0");
				headers.Add("sec-ch-ua-platform", "\"Windows\"");
				headers.Add("sec-fetch-dest", "empty");
				headers.Add("sec-fetch-mode", "cors");
				headers.Add("sec-fetch-site", "same-origin");
				headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36");
			},
			configureSerialization: config => {
			config.UseNewtonsoftJson(new JsonSerializerSettings {
				Error = (sender, args) => {
					args.ErrorContext.Handled = true;
				}
			});
		});
	}
	
	public async Task<RestResponse<AuthorizationResponse>> AuthorizeAsync(string login, string password, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/account/rawauth", Method.Post);
		req.AddQueryParameter("rawEmail", login);
		req.AddQueryParameter("rawPassword", password);
		var authResult = await _client.ExecutePostAsync<AuthorizationResponse>(req, ct);
		if (authResult.Cookies?.Count > 0)
			_client.AddDefaultHeaders(authResult.Cookies.ToDictionary(x => x.Name, z => z.Value));
		// foreach (var header in authResult.Headers) {
		// 	_client.AddDefaultHeaders();
		// }
		var newCookie = authResult.Headers.Where(x => x.Name.ToLower() == "set-cookie").Aggregate(string.Empty, (s, parameter) => s + "; " + parameter.Value);
		_client.AddDefaultHeader("Cookie", newCookie);
		return authResult;
	}
	
	public async Task<RestResponse<CatalogResponse>> GetSellerProductsAsync(
		int sellerId, int page = 1, int pageLength = 200,
		string state = "PUBLISHED", CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/catalog/products");
		req.AddQueryParameter("sellerId", sellerId);
		req.AddQueryParameter("page", page);
		req.AddQueryParameter("pageLength", pageLength);
		req.AddQueryParameter("state", state);
		return await _client.ExecuteGetAsync<CatalogResponse>(req, ct);
	}
	
	public async Task<RestResponse<ProductDataResponse>> GetProductDataAsync(
		long productId, bool withSizeChart = false, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/catalog/products/{productId}");
		req.AddQueryParameter("withSizeChart", withSizeChart);
		return await _client.ExecuteGetAsync<ProductDataResponse>(req, ct);
	}
	
	public async Task<RestResponse> CreateCommentAsync(string commentText, int productId, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/comments/base64-images", Method.Post);
		req.AddJsonBody(new { productId, text = commentText, imagesBase64 = ArraySegment<string>.Empty });
		return await _client.ExecuteAsync<AuthorizationResponse>(req, ct);
	}
	
	public async Task<RestResponse> RemoveCommentAsync(int commentId, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/comments/{commentId}", Method.Delete);
		return await _client.ExecuteAsync(req, ct);
	}
}