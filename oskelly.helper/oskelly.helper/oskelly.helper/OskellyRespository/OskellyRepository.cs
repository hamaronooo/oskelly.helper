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
		_client = new RestClient(new RestClientOptions("https://oskelly.ru"), configureSerialization: config => {
			config.UseNewtonsoftJson(new JsonSerializerSettings {
				Error = (sender, args) => {
					args.ErrorContext.Handled = true;
				}
			});
		});
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

	public async Task<RestResponse<AuthorizationResponse>> AuthorizeAsync(string login, string password, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/account/rawauth", Method.Post);
		req.AddQueryParameter("rawEmail", login);
		req.AddQueryParameter("rawPassword", password);
		return await _client.ExecutePostAsync<AuthorizationResponse>(req, ct);
	}

	public async Task<RestResponse> CreateCommentAsync(string commentText, int productId, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/comments/base64-images", Method.Post);
		req.AddJsonBody(new { productId, text = commentText });
		return await _client.ExecutePostAsync<AuthorizationResponse>(req, ct);
	}
	
	public async Task<RestResponse> RemoveCommentAsync(int commentId, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/comments/{commentId}", Method.Delete);
		return await _client.ExecutePostAsync<AuthorizationResponse>(req, ct);
	}
}