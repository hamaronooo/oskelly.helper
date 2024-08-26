using System.Text;

namespace comments.creator;

public static class RandomMessageGenerator
{
	private static Dictionary<int, string[]> _sets = new() {
		{1, []}, {2, []}, {3, []},
	};
	static RandomMessageGenerator()
	{
		_sets[1] = File.ReadAllLines(Path.Combine("comments", "1.txt"));
		_sets[2] = File.ReadAllLines(Path.Combine("comments", "2.txt"));
		_sets[3] = File.ReadAllLines(Path.Combine("comments", "3.txt"));
	}

	public static string GetRandomMessage(int? seed = null)
	{
		var random = seed is null ? Random.Shared : new Random(seed.Value);
		StringBuilder bldr = new();
		int[] setClaimed = new int[3];
		for (int i = 1; i <= 3; i++) {
			if (random.Next(0,3) == 1) continue;
			var set = GetSet();
			var add = set[random.Next(set.Length)];
			if (random.Next(0, 2) == 1) {
				bldr.Append(LatinMixer.Mix(add));
			}
			else bldr.Append(add);
			bldr.Append(random.Next(0,3) == 1 ? "\n" : " ");
		}
		return bldr.Length == 0 ? GetRandomMessage(seed) : bldr.ToString();

		string[] GetSet()
		{
			var a = random.Next(1, 4);
			if (setClaimed[a - 1] == a) return GetSet();
			setClaimed[a - 1] = a;
			return _sets[a];
		}
	}
}