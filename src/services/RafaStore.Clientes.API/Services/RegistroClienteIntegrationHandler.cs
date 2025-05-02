using FluentValidation.Results;
using RafaStore.Clientes.API.Application.Commands;
using RafaStore.Core.Mediator;
using RafaStore.Core.Messages.Integration;
using RafaStore.MessageBus;

namespace RafaStore.Clientes.API.Services;

public class RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request
            => await RegistrarCliente(request));

        return Task.CompletedTask;
    }

    private async Task<ResponseMessage> RegistrarCliente(
        UsuarioRegistradoIntegrationEvent message)
    {
        var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        var sucesso = await mediator.EnviarComando(clienteCommand);
        
        return new ResponseMessage(sucesso);
    }
}