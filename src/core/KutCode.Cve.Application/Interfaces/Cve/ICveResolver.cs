namespace KutCode.Cve.Application.Interfaces.Cve;

/// <summary>
/// Service, that finds CVE vulnerabilities and resolves.
/// In addition, may found some CVE description updates.
/// </summary>
public interface ICveResolver
{
	public string Code { get; }
	public Uri Uri { get; } // make compare by .Host
	Task<IEnumerable<VulnerabilityPointEntity>> ResolveAsync(CveId cveId, CancellationToken ct = default);
}

public sealed record CveResolverListItem(string Code, string Name, string? Domain = null, bool Enabled = true);

public sealed class CveResolverAttribute : Attribute
{
	public string Code { get; }
	public string Name { get; }
	public string? Domain { get; }
	public bool Enabled { get; }

	public CveResolverAttribute(string code, string name, string? domain = null, bool enabled = true)
	{
		Code = code;
		Name = name;
		Domain = domain;
		Enabled = enabled;
	}
	
}