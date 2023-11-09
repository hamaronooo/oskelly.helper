namespace KutCode.Cve.Application.Interfaces.Cve;

public interface ICveResolverManager
{
	Optional<ICveResolver> GetResolver(string resolverCode);
	
	/// <summary>
	/// Описание и метадата резолверов для удобного выбора в интерфейсе 
	/// </summary>
	IEnumerable<CveResolverListItem> GetResolversData();
}