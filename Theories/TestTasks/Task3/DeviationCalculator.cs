using System.Numerics;
using System.Runtime.InteropServices;

namespace TestTasks.Task3;

/// <summary>
/// 3.	Please write code that calculates standard deviation for a given list of values.
/// </summary>
public class DeviationCalculator
{
	public static double CalculateStandardDeviation(IEnumerable<double> values)
	{
		var materialized = values.ToArray();
		if (materialized.Length <= 1) return 0;
		var upperSum = materialized
			.Select(x => Math.Pow(x - materialized.Average(), 2))
			.Sum();
		return Math.Sqrt(upperSum/(materialized.Length - 1));
	}
}