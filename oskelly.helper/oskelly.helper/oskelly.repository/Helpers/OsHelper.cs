namespace oskelly.repository.Helpers;

public class OsHelper
{
	public static readonly string[] OS = new[] {
		"Windows", "MacOs", "OsX", "Linux", "Ubuntu", "Kali", "CentOS", "WindowsServer", "Debian"
	};

	public static string GetRandomOs() => OS[Random.Shared.Next(OS.Length)];
}