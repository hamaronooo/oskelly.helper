namespace KutCode.Cve.Application.Interfaces;

public interface IResolveFinderManager
{
	IResolveFinder? GetFinder(string founderCode);
}