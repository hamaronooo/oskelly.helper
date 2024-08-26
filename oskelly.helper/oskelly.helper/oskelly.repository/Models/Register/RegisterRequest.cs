namespace oskelly.repository.Models.Register;

public sealed record RegisterRequest
{
	/// <summary>
	/// 3-15 symbols length 
	/// </summary>
	public string RegisterNickname {get;set;} 
	public string RegisterEmail {get;set;}
	public string RegisterPassword {get;set;}
	public bool SubscriptionApprove { get; set; } = true;
}