
using KutCode.Cve.Application.Interfaces.Cve;

namespace KutCode.Cve.Services.CveResolve;

[CveResolver("redhat", "Центр безопасности продуктов Red Hat", "access.redhat.com")]
public sealed class RedhatCveResolver : ICveResolver
{
	public string Code => "redhat";
	public Uri Uri => new("https://access.redhat.com/");
	public async Task<IEnumerable<VulnerabilityPointEntity>> ResolveAsync(CveId cveId, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}