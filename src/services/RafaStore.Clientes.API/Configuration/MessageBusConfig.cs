using RafaStore.Clientes.API.Services;
using RafaStore.Core.Utils;
using RafaStore.MessageBus;

namespace RafaStore.Clientes.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<RegistroClienteIntegrationHandler>();
    }
}