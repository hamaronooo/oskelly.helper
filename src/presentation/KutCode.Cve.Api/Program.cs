using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using KutCode.Cve.Api.Configuration;
using KutCode.Cve.Api.Hosted;
using KutCode.Cve.Application;
using KutCode.Cve.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddFastEndpoints()
	.ConfigureSwagger()
	.ConfigureSerilogLogging()
	.AddMassTransitConfiguration();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(KutCode.Cve.Application.AssemblyInfo).Assembly));

builder.Services.AddMainDbContext(builder.Configuration.GetConnectionString("Main")!);
builder.Services.AddHostedService<WarmUpService>();
builder.Services.AddServices();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(KutCode.Cve.Domain.AssemblyInfo));

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints(c => {
	c.Endpoints.RoutePrefix = "api";
	c.Versioning.Prefix = "v";
	c.Versioning.PrependToRoute = true;
	c.Serializer.Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
app.UseSwaggerGen();

app.Urls.Add(app.Configuration.GetRequiredSection("ListenOn").Get<string>()!);

app.Run();