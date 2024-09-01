
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks;

Console.WriteLine("Benchmarks!");

BenchmarkRunner.Run<SortingBenchmark>(ManualConfig
	.Create(DefaultConfig.Instance)
	.WithOption(ConfigOptions.DisableLogFile, true)
);

