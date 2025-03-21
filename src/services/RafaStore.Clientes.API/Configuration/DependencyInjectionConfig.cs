using RafaStore.Clientes.API.Data;

namespace RafaStore.Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<ClientesContext>();
    }
}