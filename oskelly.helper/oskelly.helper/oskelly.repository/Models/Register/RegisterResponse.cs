namespace oskelly.repository.Models.Register;

public sealed record RegisterResponse
{
	public string Message { get; init; }
	/// <summary>
	/// probably is User ID
	/// </summary>
	public int Data { get; init; }
	public long TimeStamp { get; init; }
	public bool Success { get; init; }
}