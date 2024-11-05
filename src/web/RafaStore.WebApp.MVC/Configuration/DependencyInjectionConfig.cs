using RafaStore.WebApp.MVC.Extensions;
using RafaStore.WebApp.MVC.Services;
using RafaStore.WebApp.MVC.Services.Handlers;

namespace RafaStore.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
        
        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

        services.AddHttpClient<ICatalogoService, CatalogoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUser, AspNetUser>();
    }
}