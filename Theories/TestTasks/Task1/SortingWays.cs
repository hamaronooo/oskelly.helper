using System.ComponentModel;

namespace TestTasks.Task1;

/// <summary>
/// 1.	There is an array of integer. Size of the array can vary from 0 to N where N is definitely less than int.MaxValue.
/// Please write code that sorts a given array from max value to min. What is the complexity of the algorithm?
/// </summary>
public class SortingWays
{
	private static int[] InitArray(int length = 0) => Enumerable
		.Range(0, length < 0 ? Random.Shared.Next(0, int.MaxValue) : length)
		.Select(x => Random.Shared.Next()).ToArray();
	
	public static int[] Linq(int length)
	{
		return InitArray(length).OrderDescending().ToArray();
	}

	public static int[] CustomSort(int length)
	{
		// avg complexity: logN*N
		return CustomSorter.Sort(InitArray(length), CustomSorter.SortDirection.Descending, CustomSorter.KnownArrayState.Chaotic);
	}
	
/*

| Method     | Length  | Mean         | Error        | StdDev       | Gen0      | Gen1      | Gen2     | Allocated  |
|----------- |-------- |-------------:|-------------:|-------------:|----------:|----------:|---------:|-----------:|
| Linq       | 1000    |     31.37 us |     0.471 us |     0.393 us |    0.9155 |         - |        - |    7.98 KB |
| CustomSort | 1000    |     34.31 us |     2.295 us |     2.254 us |    0.4883 |         - |        - |    4.02 KB |
| Linq       | 1000000 | 64,194.77 us | 2,972.288 us | 2,780.280 us | 1222.2222 | 1222.2222 | 666.6667 | 7813.17 KB |
| CustomSort | 1000000 | 60,314.01 us | 2,058.692 us | 1,925.702 us |  777.7778 |  777.7778 | 222.2222 | 3906.58 KB |

*/
}