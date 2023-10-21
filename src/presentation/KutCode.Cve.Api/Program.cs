using KutCode.Cve.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMainDbContext(builder.Configuration.GetConnectionString("Main")!);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();