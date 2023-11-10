namespace KutCode.Cve.Application.Interfaces.Cve;

/// <summary>
/// Service, that finds CVE vulnerabilities and resolves.
/// In addition, may found some CVE description updates.
/// </summary>
/// <remarks>
/// It's required to add <see cref="CveResolverAttribute" /> to inherited class declaration.
/// Needed for search with <see cref="ICveResolverManager"/> work.
/// </remarks>
public interface ICveResolver
{
	public string Code { get; }
	Task<IEnumerable<VulnerabilityPointEntity>> ResolveAsync(CveId cveId, CancellationToken ct = default);
}

/// <summary>
/// DTO for display existed <see cref="ICveResolver"/>s
/// </summary>
/// <param name="Code">Unique Code of <see cref="ICveResolver"/></param>
/// <param name="Name">Human readable name of <see cref="ICveResolver"/></param>
/// <param name="Domain">Site where loading from domain name or something like that</param>
/// <param name="Enabled">Is enabled for using</param>
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