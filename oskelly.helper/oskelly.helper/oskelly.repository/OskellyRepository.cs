using System.Net;
using Newtonsoft.Json;
using oskelly.repository.Helpers;
using oskelly.repository.Models;
using oskelly.repository.Models.Catalog;
using oskelly.repository.Models.ProductData;
using oskelly.repository.Models.Register;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace oskelly.repository;

public class OskellyRepository
{
	private readonly RestClient _client;
	private readonly OskellyRepositorySettings _settings;
	
	public OskellyRepository(OskellyRepositorySettings settings)
	{
		_settings = settings;
		_client = new RestClient(new RestClientOptions(settings.BaseUri),
			configureDefaultHeaders: headers => {
				headers.Add("sec-ch-ua", $"""Not)A;Brand";v="{Random.Shared.Next(50, 100)}", "Google Chrome";v="{Random.Shared.Next(50, 150)}", "Chromium";v="{Random.Shared.Next(50, 150)}""");
				headers.Add("sec-ch-ua-mobile", "?0");
				headers.Add("sec-ch-ua-platform", $"\"{_settings.OperationSystem}\"");
				headers.Add("sec-fetch-dest", "empty");
				headers.Add("sec-fetch-mode", "cors");
				headers.Add("sec-fetch-site", "same-origin");
				headers.Add("user-agent", _settings.UserAgent);
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
		if (authResult.IsSuccessful)
			_client.SetHeadersFromResponse(authResult);
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
		var data = new byte[4];
		new Random().NextBytes(data);
		IPAddress ip = new IPAddress(data);
		var req = new RestRequest($"api/v2/comments/base64-images", Method.Post);
		req.AddHeader("x-forwarded-for", ip.ToString());
		req.AddJsonBody(new { productId, text = commentText, imagesBase64 = ArraySegment<string>.Empty });
		return await _client.ExecuteAsync<AuthorizationResponse>(req, ct);
	}
	
	public async Task<RestResponse> RemoveCommentAsync(int commentId, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/comments/{commentId}", Method.Delete);
		return await _client.ExecuteAsync(req, ct);
	}
	
	/// <summary>
	/// Register new user and set cookies if success
	/// </summary>
	public async Task<RestResponse<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
	{
		var req = new RestRequest($"api/v2/account/register", Method.Post);
		req.AddHeader(KnownHeaders.ContentType, "application/x-www-form-urlencoded; charset=UTF-8");
		req.AddParameter("application/x-www-form-urlencoded", 
			$"registerNickname={request.RegisterNickname}" +
			$"&registerEmail={request.RegisterEmail}" +
			$"&registerPassword={request.RegisterPassword}&registerConfirmPassword={request.RegisterPassword}" +
			$"&subscriptionApprove={request.SubscriptionApprove}",
			ParameterType.RequestBody);

		var result = await _client.ExecuteAsync<RegisterResponse>(req, ct);
		if (result.IsSuccessful)
			_client.SetHeadersFromResponse(result);
		return result;
	}

	public Task<RestResponse<string>> CheckIpAsync()
	{
		return _client.ExecuteGetAsync<string>("https://api.ipify.org/");
	}
	
}