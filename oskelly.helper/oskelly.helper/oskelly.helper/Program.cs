using Humanizer;
using oskelly.helper;
using oskelly.helper.OskellyRespository.Models;
using oskelly.helper.Settings;
using RestSharp;

ConsoleHelper.WriteWelcome();

string? commentToRemove = FileHelper.GetRemoveCommentText();
string? commentToCreate = FileHelper.GetCreateCommentText();

if (string.IsNullOrWhiteSpace(commentToRemove) && string.IsNullOrWhiteSpace(commentToCreate)) {
	Console.WriteLine("BOTH COMMENTS ARE EMPTY... WRITE ATLEAST ONE AND RERUN APP");
	Console.Read();
	return;
}

if (string.IsNullOrWhiteSpace(commentToRemove))
	ConsoleHelper.ShowRemoveCommentNotFound();
if (string.IsNullOrWhiteSpace(commentToCreate))
	ConsoleHelper.ShowCreateCommentNotFound();

RestResponse<AuthorizationResponse>? authResponse = null;
var repo = new OskellyRepository();
(string login, string password)? logPass = null;
if (AppSettingsManager.Exists) {
	Console.WriteLine("Using existed credentials!");
	logPass = (AppSettingsManager.Settings.Login, AppSettingsManager.Settings.Password);
}
if (authResponse is null || !authResponse.IsSuccessful)
{
	while (authResponse == null || !authResponse.IsSuccessful || authResponse.Data is {Success: true}) {
		if (logPass.HasValue is false)
			logPass = ConsoleHelper.GetLoginAndPassword();
		Console.Clear();
		authResponse = await repo.AuthorizeAsync(logPass.Value.login, logPass.Value.password);
		if (authResponse.IsSuccessful && authResponse.Data is {Success: true})
			break;
		ConsoleHelper.ShowFailedAuthBanner(authResponse.Data!);
		ConsoleHelper.ClearLastLine();
	}
}
ConsoleHelper.ShowSuccessAuthBanner(authResponse.Data!);
AppSettingsManager.Override(new AppSettings {Login = logPass.Value.login, Password = logPass.Value.password});

var catalogResponse = await repo.GetSellerProductsAsync(authResponse.Data.Data.Id);
Console.WriteLine($"=== Loaded {catalogResponse.Data?.Data.Items.Count} items from this account ===");
if (catalogResponse.Data?.Data.Items.Count == 0) {
	Console.WriteLine("NO ITEMS FOUND IN PROFILE... ADD SOME AND RERUN APP");
	Console.Read();
	return;
}

foreach (var item in catalogResponse.Data?.Data.Items!) {
	for (int i = 0; i < 40; i++) {
		await Task.Delay(25);
		Console.Write("=");
	}
	Console.WriteLine();
	Console.WriteLine($"{item.Description.Replace('\n', ' ').Replace('\r', ' ').Truncate(50)} | {item.Brand.Name} | {item.Name}", Console.ForegroundColor = ConsoleColor.Cyan);
	Console.ResetColor();

	var productData = await repo.GetProductDataAsync(item.ProductId);
	if (productData.IsSuccessful is false || productData.Data is null) {
		continue;
	}
	if (!string.IsNullOrWhiteSpace(commentToRemove)) {
		/// removing
		var myComments = productData.Data?.Data.Comments.Where(x => x.UserId == authResponse.Data.Data.Id).ToList();
		Console.WriteLine($"Found {myComments?.Count} comments of mine");
		
		var oldTrimmedComment = commentToRemove.Trim().Replace("\r", string.Empty).Replace("\t", string.Empty)
			.Replace("\n", string.Empty)
			.Replace(" ", string.Empty);
		foreach (var comment in myComments) {
			var trimmedComment = comment.Text.Trim().Replace("\r", string.Empty).Replace("\t", string.Empty)
				.Replace("\n", string.Empty)
				.Replace(" ", string.Empty);
			if (oldTrimmedComment == trimmedComment) {
				var response = await repo.RemoveCommentAsync(comment.Id);
				if (response.IsSuccessful) {
					Console.WriteLine("🗑️ Removed old comment", Console.ForegroundColor = ConsoleColor.Green);
					Console.ResetColor();
				}
				else {
					Console.WriteLine($"⚠️ Failed to remove old comment; Status: {response.StatusCode}", Console.ForegroundColor = ConsoleColor.Red);
					Console.ResetColor();
				}
			}
		}
	}
	
	/// creating 
	if (!string.IsNullOrWhiteSpace(commentToCreate)) {
		var response = await repo.CreateCommentAsync(commentToCreate, item.ProductId);
		if (response.IsSuccessful) {
			Console.WriteLine("✨ Created new comment", Console.ForegroundColor = ConsoleColor.Green);
			Console.ResetColor();
		}
		else {
			Console.WriteLine($"⚠️ Failed to create new comment; Status: {response.StatusCode}", Console.ForegroundColor = ConsoleColor.Red);
			Console.ResetColor();
		}
	}
}

Console.WriteLine();
Console.WriteLine();
Console.WriteLine("========  JOB FINISHED  ========");