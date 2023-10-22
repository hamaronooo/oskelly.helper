namespace KutCode.Cve.Application.Interfaces;

public interface ICveCache
{
	bool IsExist(CveId cveId);
	void Add(CveId cveId);
	void AddRange(IEnumerable<CveId> cve);
}