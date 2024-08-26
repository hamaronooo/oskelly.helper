namespace comments.creator;

public class LatinMixer
{
	public static string Mix(string input)
	{
		char[] chars = input.ToCharArray();
		for (int i = 0; i < chars.Length; i++) {
			if (_latins.TryGetValue(chars[i], out var replaceChar))
				chars[i] = replaceChar;
		}
		return new string(chars);
	}
	
	private static Dictionary<char, char> _latins = new() {
		{'с', 'c'},
		{'а', 'a'},
		{'о', 'o'},
		{'х', 'x'},
		{'у', 'y'},
		{'Х', 'X'},
		{'С', 'C'},
		{'Н', 'H'},
		{'Р', 'P'},
		{'А', 'A'},
		{'О', 'O'},
		{'Т', 'T'},
		{'В', 'B'},
		{'З', '3'},
		{'Е', 'E'},
	};
}