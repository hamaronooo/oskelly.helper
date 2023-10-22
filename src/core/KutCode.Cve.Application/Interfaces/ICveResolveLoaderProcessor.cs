namespace KutCode.Cve.Application.Interfaces;

public interface ICveResolveLoaderProcessor
{
	void Push(CveId cveId, string founderCode);
	Task StartAsync(CancellationToken ct = default);
}