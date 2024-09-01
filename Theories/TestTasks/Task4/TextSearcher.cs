using System.Text.RegularExpressions;

namespace TestTasks.Task4;

/// <summary>
/// 4.	There is a text that contains N symbols, N is definitely less than 2^16.
/// Please write code that returns count of symbols “A” in the text where symbol “B” is 3 positions before “A” and symbol “C” is right next after “A”. 
/// </summary>
public class TextSearcher
{
	private static Regex _myRegex = new ("B.{2}AC", RegexOptions.Compiled, TimeSpan.FromSeconds(2));
	public static int RegexCountPatternEntries(string input)
	{
		return _myRegex.Count(input);
	}
	
	
	// self-made parsing, for test 
	public static int CountPatternEntries(string input)
	{
		int counter = 0;
		int maxIndex = input.Length - 1;
		for (int i = 0; i < input.Length; i++) {
			if (input[i] == 'B'
			    && i + 4 <= maxIndex
			    && input[i + 3] == 'A'
			    && input[i + 4] == 'C') 
			{
				counter++;
				if (i + 8 <= maxIndex) i += 3;
			}
		}
		return counter;
	}
	
/*
conclusion: regex calculations is faster at least in 2 times;

| Method | Length | Mean         | Error     | StdDev    | Allocated |
|------- |------- |-------------:|----------:|----------:|----------:|
| Regex  | 100    |     24.63 ns |  0.145 ns |  0.121 ns |         - |
| Custom | 100    |     29.62 ns |  0.240 ns |  0.212 ns |         - |
| Regex  | 6000   |    530.46 ns |  2.197 ns |  1.947 ns |         - |
| Custom | 6000   |  1,959.67 ns | 12.132 ns | 11.349 ns |         - |
| Regex  | 65536  |  4,991.04 ns | 14.856 ns | 13.170 ns |         - |
| Custom | 65536  | 24,156.33 ns | 60.586 ns | 56.673 ns |         - |
   
*/
}