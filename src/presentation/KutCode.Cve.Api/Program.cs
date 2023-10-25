global using Serilog;
global using FastEndpoints;
global using KutCode.Cve.Services;
global using KutCode.Cve.Application;
global using MediatR;
global using AutoMapper;
using System.Text.Json.Serialization;
using FastEndpoints.Swagger;
using KutCode.Cve.Api.Configuration;
using KutCode.Cve.Api.Hosted;
using KutCode.Cve.Application.Database;
using KutCode.Cve.Excel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddFastEndpoints()
	.ConfigureCors()
	.ConfigureSwagger()
	.ConfigureSerilogLogging();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(KutCode.Cve.Application.AssemblyInfo).Assembly));
builder.Services.AddMainDbContext(builder.Configuration.GetConnectionString("Main")!);
builder.Services.AddHostedService<WarmUpService>();
builder.Services.AddHostedService<LoaderProcessorService>();
builder.Services.AddCveFinderProcessor();
builder.Services.AddServices();
builder.Services.AddFileService(builder.Environment.WebRootPath);
builder.Services.AddExcelServices();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(KutCode.Cve.Domain.AssemblyInfo), typeof(Program));

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(CorsConfiguration.CorsPolicyName);

app.UseDefaultExceptionHandler();
app.UseFastEndpoints(c => {
	c.Endpoints.RoutePrefix = "api";
	c.Versioning.Prefix = "v";
	c.Versioning.PrependToRoute = true;
	c.Serializer.Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
app.UseSwaggerGen();

app.Urls.Add(app.Configuration.GetRequiredSection("ListenOn").Get<string>()!);

{
	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
	await context.Database.MigrateAsync();
	Log.Information("DB migrated success!");
}
Log.Information("Starting application...");
await app.RunAsync();