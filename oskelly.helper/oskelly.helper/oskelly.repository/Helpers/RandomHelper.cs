namespace oskelly.repository.Helpers;

public class RandomHelper
{
	const string AlphaNumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	const string NumericChars = "0123456789";
	
	public static Span<char> RandomAlphaNumericString(int length) => new(Enumerable.Repeat(AlphaNumericChars, length)
		.Select(s => s[Random.Shared.Next(s.Length)]).ToArray());

	public static string RandomGuidString() => Guid.NewGuid().ToString();

	public static Span<char> RandomNumberString(int length)
	{
		char[] array = new char[length];
		for (int i = 0; i < length; i++)
			array[i] = NumericChars[Random.Shared.Next(NumericChars.Length)];
		if (array[0] == '0')
			array[0] = '1';
		return array;
	}
}