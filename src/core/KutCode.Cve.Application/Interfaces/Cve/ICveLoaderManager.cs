namespace KutCode.Cve.Application.Interfaces.Cve;

public interface ICveLoaderManager
{
	ICveLoader? GetLoader(string loaderCode);
}