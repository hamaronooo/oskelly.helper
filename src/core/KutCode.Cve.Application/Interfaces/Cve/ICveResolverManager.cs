namespace KutCode.Cve.Application.Interfaces.Cve;

public interface ICveResolverManager
{
	ICveResolver? GetResolver(string resolverCode);
}