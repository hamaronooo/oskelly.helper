using BenchmarkDotNet.Attributes;
using TestTasks.Task1;

namespace Benchmarks;

[MemoryDiagnoser]
[MaxIterationCount(16)]
public class SortingBenchmark
{
	[Params(1_000, 1_000_000)]
	public int Length { get; set; }
	
	[Benchmark]
	public int[] Linq() => SortingWays.Linq(Length);	
	
	[Benchmark]
	public int[] CustomSort() => SortingWays.CustomSort(Length);
}