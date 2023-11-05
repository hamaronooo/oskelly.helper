namespace KutCode.Cve.Api.Middlewares;

public sealed class ResponseMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{ 
			await next.Invoke(context);
		}
		catch (Exception e) {
			await context.Response.WriteAsJsonAsync(new ApiResponse
			{
				StatusCode = context.Response.StatusCode,
				Error = e.Message
			});
		}
	}
}

public sealed class ApiResponse<T> : ApiResponse
{
	public T? Data { get; set; }
}
public class ApiResponse
{
	public int StatusCode { get; set; }
	public string? Error { get; set; }
}