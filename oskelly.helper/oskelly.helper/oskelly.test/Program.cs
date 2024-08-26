// See https://aka.ms/new-console-template for more information

using comments.creator;
using oskelly.repository;
using oskelly.repository.Helpers.Identity;
using oskelly.repository.Models.Register;

Console.WriteLine(RandomIdentityGenerator.GetRandomNickName());
return;
Console.WriteLine("Hello, World!");
//
// HttpClient client = new();
//
// var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://oskelly.ru"));

var repo = new OskellyRepository(OskellyRepositorySettings.Random);
var regReq = new RegisterRequest {
	RegisterEmail = "irinavaginvna91112@gmail.com",
	RegisterNickname = "Irinag3132",
	RegisterPassword = "Irar33Rr"
};
var regResponse = await repo.RegisterAsync(regReq);
var profile = await repo.GetSellerProductsAsync(238153);
var prod = profile.Data?.Data.Items.First();
var comment = await repo.CreateCommentAsync("Товар в наличие в мск?", prod.ProductId);

Console.WriteLine(111);
