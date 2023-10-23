namespace KutCode.Cve.Application.Interfaces.Cve;

public interface ICveCache
{
	bool IsExist(CveId cveId);
	void Add(CveId cveId);
	void AddRange(IEnumerable<CveId> cve);
}