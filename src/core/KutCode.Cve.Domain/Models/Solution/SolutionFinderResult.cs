using KutCode.Optionality;

namespace KutCode.Cve.Domain.Models.Solution;

public sealed class SolutionFinderResult<TResolve> where TResolve : class
{
	public SolutionFinderResult() { }
	public SolutionFinderResult(IEnumerable<SolutionFinderResultItem<TResolve>> resolves)
	{
		Resolves = resolves as List<SolutionFinderResultItem<TResolve>> ?? resolves.ToList();
	}
	public List<SolutionFinderResultItem<TResolve>> Resolves { get; set; } = new();
	public Optional<TResolve> Best => Optional.From(Resolves.MaxBy(x => x.Score)?.Resolve);
}