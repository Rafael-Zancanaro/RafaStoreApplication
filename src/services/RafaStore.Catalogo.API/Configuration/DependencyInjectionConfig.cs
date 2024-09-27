using RafaStore.Catalogo.API.Data.Repository;
using RafaStore.Catalogo.API.Data;
using RafaStore.Catalogo.API.Models;

namespace RafaStore.Catalogo.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<CatalogoContext>();
    }
}
