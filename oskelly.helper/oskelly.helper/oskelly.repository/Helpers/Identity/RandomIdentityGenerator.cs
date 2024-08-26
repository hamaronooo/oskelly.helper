using System.Text;
using oskelly.repository.Models.Register;

namespace oskelly.repository.Helpers.Identity;

public class RandomIdentityGenerator
{
	private static string[] _names { get; set; }
	static RandomIdentityGenerator()
	{
		_names = File.ReadAllLines(Path.Combine("Helpers", "Identity", "identities.txt"));
	}
	
	const string AlphaNumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
	public static RegisterRequest Get()
	{
		return new RegisterRequest {
			RegisterEmail = new string(Enumerable.Repeat(AlphaNumericChars, Random.Shared.Next(5,15))
				.Select(s => s[Random.Shared.Next(s.Length)]).ToArray()) + "@gmail.com",
			RegisterNickname = GetRandomNickName(),
			RegisterPassword = new(Enumerable.Repeat(AlphaNumericChars, Random.Shared.Next(8,14))
				.Select(s => s[Random.Shared.Next(s.Length)]).ToArray()),
		};
	}

	public static string GetRandomNickName()
	{
		var bldr = new StringBuilder();
		bldr.Append(_names[Random.Shared.Next(_names.Length)]);
		if (bldr.Length < 10) {
			bldr.Append("_");
			bldr.Append(_names[Random.Shared.Next(_names.Length)]);
		}
		while (bldr.Length < 14)
			bldr.Append(Random.Shared.Next(0, 10));
		return bldr.ToString();
	}
}