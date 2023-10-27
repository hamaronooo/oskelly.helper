
using System.Text.RegularExpressions;

namespace KutCode.Cve.Domain.Helpers;

public sealed class NamesNormalizer
{
	// regexes from https://regexlib.com/Search.aspx?k=file+path&AspxAutoDetectCookieSupport=1
	public static Regex FilePathRegex => new (@"(?:[\w]\:|\/)(\/[a-z_\-\s0-9\.]+){1,60}\.[a-zA-Z0-9]{1,10}");
	public static char[] CharsToRemove = new[] {'\"', '\'', '{', '}', '[', ']', '|', '?', ':', ';', '^', ',', '%', '#', '№'};
	public static string NormalizeSoftwareName(string software)
	{
		return FilePathRegex.Replace(software, string.Empty)
			.Trim(CharsToRemove);
	}
	
	public static string NormalizePlatformName(string platform)
	{
		return FilePathRegex.Replace(platform, string.Empty)
			.Trim(CharsToRemove);
	}
}