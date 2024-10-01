using RafaStore.Identidade.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

var services = builder.Services;
var configuration = builder.Configuration;

// Services
services.AddIdentityConfiguration(configuration);

services.AddApiConfiguration();

services.AddSwaggerConfiguration();


//App Builder
var app = builder.Build();

app.UseSwaggerConfiguration(app.Environment);

app.UseApiConfiguration(app.Environment);

app.Run();