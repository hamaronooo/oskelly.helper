using System.Text.RegularExpressions;

namespace KutCode.Cve.Domain;

public class Regexes
{
	public static Regex KbRegex = new Regex("KB[0-9]{1,10}");
}