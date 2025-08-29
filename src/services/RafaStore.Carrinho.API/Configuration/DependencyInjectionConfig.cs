using RafaStore.Carrinho.API.Data;
using RafaStore.WebAPI.Core.Usuario;

namespace RafaStore.Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<CarrinhoContext>();
    }
}