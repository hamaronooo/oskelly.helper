namespace KutCode.Cve.Application.Interfaces.Cve;

public interface ICveResolverManager
{
	Optional<ICveResolver> GetResolver(string resolverCode);
}