using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Polly;
using RafaStore.WebAPI.Core.Usuario;
using RafaStore.WebApp.MVC.Extensions;
using RafaStore.WebApp.MVC.Services;
using RafaStore.WebApp.MVC.Services.Handlers;

namespace RafaStore.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        
        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
        
        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

        services.AddHttpClient<ICatalogoService, CatalogoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddTransientHttpErrorPolicy(
                p => p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(600)));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IAspNetUser, AspNetUser>();
    }
}