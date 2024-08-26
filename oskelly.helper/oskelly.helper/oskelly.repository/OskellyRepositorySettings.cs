using oskelly.repository.Helpers;
using RandomUserAgent;

namespace oskelly.repository;

public sealed record class OskellyRepositorySettings
{
	public static OskellyRepositorySettings Random => new() {
		OperationSystem = OsHelper.GetRandomOs(),
		UserAgent = RandomUa.RandomUserAgent
	};

	
	
	public Uri BaseUri { get; set; } = new ("https://oskelly.ru");
	public string OperationSystem { get; set; }
	public string UserAgent { get; set; }
}