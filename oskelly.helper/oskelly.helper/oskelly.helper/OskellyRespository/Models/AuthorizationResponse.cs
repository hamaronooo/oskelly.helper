using Newtonsoft.Json;

namespace oskelly.helper.OskellyRespository.Models;


public class AuthorizationResponse
{
	[JsonProperty("message")]
	public string Message { get; set; }

	[JsonProperty("data")]
	public AuthorizationData? Data { get; set; }

	[JsonProperty("timestamp")]
	public long Timestamp { get; set; }

	[JsonProperty("executionTimeMillis")]
	public int ExecutionTimeMillis { get; set; }
	
	[JsonProperty("success")]
	public bool Success { get; set; }
}


public class AuthorizationData
{
	[JsonProperty("id")]
	public int Id { get; set; }
}
