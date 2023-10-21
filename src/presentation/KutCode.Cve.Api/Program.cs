using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using KutCode.Cve.Api.Configuration;
using KutCode.Cve.Api.Hosted;
using KutCode.Cve.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddFastEndpoints().ConfigureSwagger();

builder.Services.AddMainDbContext(builder.Configuration.GetConnectionString("Main")!);
builder.Services.AddHostedService<WarmUpService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c => {
	c.Endpoints.RoutePrefix = "api";
	c.Versioning.Prefix = "v";
	c.Versioning.PrependToRoute = true;
	c.Serializer.Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
app.UseSwaggerGen();

app.Urls.Add(app.Configuration.GetRequiredSection("ListenOn").Get<string>()!);

app.Run();