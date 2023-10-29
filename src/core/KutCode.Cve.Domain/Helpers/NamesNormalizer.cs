using System.Text.RegularExpressions;

namespace KutCode.Cve.Domain.Helpers;

public sealed class NamesNormalizer
{
	// regexes from https://regexlib.com/Search.aspx?k=file+path&AspxAutoDetectCookieSupport=1
	public static Regex FilePathRegex => new (@"(([a-zA-Z]:)|((\\|\/){1,2}\w+)\$?)((\\|\/)(\w[\w ]*.*))+\.([a-zA-Z0-9]+)");

	public static HashSet<char> CharsToRemove = new(new[]
	{
		(char)0,'\0', '\"', '\'', '/', '-', '(', ')', '{', '}', '[', ']', '|', '?', ':', ';', '^', ',', '%', '#', '№', '=', '+',
		'!', '@'
	});
	
	public static string NormalizeSoftwareName(string value)
	{
		if (string.IsNullOrEmpty(value)) return string.Empty;
		return TrimFixed(FilePathRegex.Replace(value, string.Empty));
	}
	
	public static string NormalizePlatformName(string value)
	{
		if (string.IsNullOrEmpty(value)) return string.Empty;
		return TrimFixed(FilePathRegex.Replace(value, string.Empty));
	}

	// this is naive impl. todo: rework to nice performance impl. 
	private static string TrimFixed(string val)
	{
		var charArray = val.ToCharArray();
		var buffIndex = 0;
		char[] buffer = new char[charArray.Length];
		for (int i = 0; i < charArray.Length; i++) {
			if (CharsToRemove.Contains(charArray[i]))
				continue;
			buffer[buffIndex++] = charArray[i];
		}
		return new string(buffer, 0, buffIndex);
	}
}