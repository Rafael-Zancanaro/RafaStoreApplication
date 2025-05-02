using FluentValidation.Results;
using MediatR;
using RafaStore.Clientes.API.Application.Commands;
using RafaStore.Clientes.API.Application.Events;
using RafaStore.Clientes.API.Data;
using RafaStore.Clientes.API.Data.Repository;
using RafaStore.Clientes.API.Models;
using RafaStore.Core.Mediator;

namespace RafaStore.Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
        
        services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientesContext>();
    }
}