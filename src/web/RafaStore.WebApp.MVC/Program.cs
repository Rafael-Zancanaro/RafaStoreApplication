using RafaStore.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var services = builder.Services;

//Services
services.AddIdentityConfiguration();

services.AddMvcConfiguration(builder.Configuration);

services.RegisterServices();

//App
var app = builder.Build();

app.UseMvcConfiguration(app.Environment);

app.Run();