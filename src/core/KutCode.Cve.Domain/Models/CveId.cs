namespace KutCode.Cve.Domain.Models;

/// <summary>
/// Full name of CVE in format: CVE-YYYY-XXXX<br/>
/// Where YYYY - is CVE publishing year.
/// And XXXX - is CNA code
/// </summary>
public readonly struct CveId
{
	private const string CvePrefix = "cve";
	private readonly int _year;
	private readonly string _cnaNumber;

	/// <summary>
	/// Parse CVE id string to Object
	/// </summary>
	/// <param name="cveIdAsString">Correct CVE ID string like CVE-XXXX-YYYYY or may be XXXX-YYYYY</param>
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="FormatException"></exception>
	public static CveId Parse(string cveIdAsString)
	{
		if (string.IsNullOrEmpty(cveIdAsString)) throw new ArgumentNullException(nameof(cveIdAsString));
		var trimmed = cveIdAsString.Trim();
		if (trimmed.Length > 17)  throw new ArgumentException("String is too long for CVE id");
		var parts = trimmed.Split('-');
		if (parts.Length is > 3 or < 2) throw new ArgumentException("Wrong CVE id string");
		var firstPart = parts[0].Trim().ToLower();
		if (firstPart == CvePrefix)
			return new CveId(int.Parse(parts[1]), parts[2]);
		
		if (firstPart != CvePrefix && parts.Length > 2) throw new ArgumentException("Wrong CVE id string");
		return new CveId(int.Parse(parts[0]), parts[1]);
	}
	
	public CveId(int year, string cnaNumber)
	{
		_year = year;
		_cnaNumber = cnaNumber;
	}

	public string AsString => $"CVE-{_year}-{_cnaNumber}";

	/// <summary>
	/// Year of CVE publication<br/>
	/// Used on Second place in CVE ID: CVE-YYYY-....
	/// </summary>
	public int Year => _year;

	/// <summary>
	/// CVE Numbering Authorities<br/>
	/// Used on third place in CVE ID: CVE-....-XXXX
	/// </summary>
	public string CnaNumber => _cnaNumber;

	public override string ToString() => AsString;
}