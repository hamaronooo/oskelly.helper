namespace KutCode.Cve.Domain.Models.Solution;

public sealed class SolutionFinderResultItem<TResolve>
{
	public SolutionFinderResultItem(TResolve resolve, double score)
	{
		Resolve = resolve;
		Score = score;
	}
	public TResolve Resolve { get; set; }
	public double	Score { get; set; }
}