using comments.creator;
using Microsoft.VisualBasic;
using oskelly.repository;
using oskelly.repository.Helpers;
using oskelly.repository.Helpers.Identity;

// reg page https://oskelly.ru/login


Console.WriteLine("SPAM IT ALL!");

int success = 0;
while (success != 3) {
	OskellyRepository repo = new(OskellyRepositorySettings.Random);
	Console.WriteLine($"My IP: {(await repo.CheckIpAsync()).Content}");
	if (!await RegisterNew())
		continue;

	// shirshova 48615
	// tony 466079
	// luxurybutjunkie 539458
	var products = await repo.GetSellerProductsAsync(466079, 1, 800);
	var items = products.Data.Data.Items.ToArray();
	Console.WriteLine($"Loaded {products.Data.Data.ItemsCount}");
	for (int i = 0; i < items.Length; i++) {
		for (int j = 0; j < 5; j++) {
			await Task.Delay(100);
			Console.Write("=");
		}
		var response = await repo.CreateCommentAsync(RandomMessageGenerator.GetRandomMessage(), items[i].ProductId);
		Console.WriteLine($" {i:0000}/{items.Length} Added: {response.StatusCode}");
		if (i % 5 == 0 && items.Length > 10 && i > 0) {
			while ((await RegisterNew()) == false) {
				//swallow
			}
		}
	}

	success++;

	async Task<bool> RegisterNew()
	{
		var identity = RandomIdentityGenerator.Get();
		Console.WriteLine($"{identity}");
		var res = await repo.RegisterAsync(identity);
		Console.WriteLine($"Authorization status: {res.StatusCode}; {res.Content}");
		Console.WriteLine($"Auth: {identity}");
		return res.IsSuccessful;
	}
}

Console.WriteLine("=== FINISH ===");