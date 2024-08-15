using Humanizer;
using oskelly.helper;
using oskelly.helper.OskellyRespository.Models;
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
while (authResponse == null || !authResponse.IsSuccessful || authResponse.Data is {Success:true}) {
	var logPass = ConsoleHelper.GetLoginAndPassword();
	Console.Clear();
	authResponse = await repo.AuthorizeAsync(logPass.login, logPass.password);
	if (authResponse.IsSuccessful && authResponse.Data is {Success: true})
		break;
	ConsoleHelper.ShowFailedAuthBanner(authResponse.Data!);
	ConsoleHelper.ClearLastLine();
}
ConsoleHelper.ShowSuccessAuthBanner(authResponse.Data!);

var catalogResponse = await repo.GetSellerProductsAsync(authResponse.Data.Data.Id);
Console.WriteLine($"=== Loaded {catalogResponse.Data?.Data.Items.Count} items from this account ===");
if (catalogResponse.Data?.Data.Items.Count == 0) {
	Console.WriteLine("NO ITEMS FOUND IN PROFILE... ADD SOME AND RERUN APP");
	Console.Read();
	return;
}

foreach (var item in catalogResponse.Data?.Data.Items!) {
	Console.WriteLine($"{item.Description.Replace('\n', ' ').Replace('\r', ' ').Truncate(34)} | {item.Brand.Name} | {item.Name}");

	var productData = await repo.GetProductDataAsync(item.ProductId);
	
}



Console.WriteLine(111);