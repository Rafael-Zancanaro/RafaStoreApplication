using RafaStore.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

//Services
services.AddIdentityConfiguration();

services.AddMvcConfiguration();

services.RegisterServices();

//App
var app = builder.Build();

app.UseMvcConfiguration(app.Environment);

app.Run();