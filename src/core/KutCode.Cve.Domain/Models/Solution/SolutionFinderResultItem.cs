namespace KutCode.Cve.Domain.Models.Solution;

public sealed class SolutionFinderResultItem<TResolve>
{
	public SolutionFinderResultItem(CveId cveId, TResolve resolve, double score)
	{
		Resolve = resolve;
		Score = score;
	}

	public CveId CveId { get; set; }
	public TResolve Resolve { get; set; }
	public double	Score { get; set; }
}