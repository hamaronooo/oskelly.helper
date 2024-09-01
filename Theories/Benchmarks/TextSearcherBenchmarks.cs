using BenchmarkDotNet.Attributes;
using TestTasks.Task4;

namespace Benchmarks;

[MemoryDiagnoser]
[MaxIterationCount(16)]
public class TextSearcherBenchmarks
{
	private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private string Input { get; set; }
	
	[Params(100, 6000, 65536)]
	public int Length { get; set; }

	[GlobalSetup]
	public void SetUp()
	{
		Input = new string(Enumerable.Range(0, Length).Select(x
			=> Alphabet[Random.Shared.Next(0, Alphabet.Length)]).ToArray());
	}
	
	[Benchmark]
	public int Regex() => TextSearcher.RegexCountPatternEntries(Input);
	
	[Benchmark]
	public int Custom() => TextSearcher.CountPatternEntries(Input);
}