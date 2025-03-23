using EasyNetQ;
using FluentValidation.Results;
using RafaStore.Clientes.API.Application.Commands;
using RafaStore.Core.Mediator;
using RafaStore.Core.Messages.Integration;

namespace RafaStore.Clientes.API.Services;

public class RegistroClienteIntegrationHandler(IServiceProvider serviceProvider) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var bus = scope.ServiceProvider.GetRequiredService<IBus>();
        
        bus.Rpc.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request
            => new ResponseMessage(await RegistrarCliente(request)), cancellationToken: stoppingToken);

        return Task.CompletedTask;
    }

    private async Task<ValidationResult> RegistrarCliente(
        UsuarioRegistradoIntegrationEvent message)
    {
        var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        return await mediator.EnviarComando(clienteCommand);
    }
}