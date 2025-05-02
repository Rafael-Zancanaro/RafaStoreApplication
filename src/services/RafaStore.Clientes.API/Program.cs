using MediatR;
using RafaStore.Clientes.API.Configuration;
using RafaStore.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddMediatR(typeof(Program));

builder.Services.RegisterServices();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

//App
var app = builder.Build();

app.UseSwaggerConfiguration(app.Environment);

app.UseApiConfiguration(app.Environment);

app.Run();