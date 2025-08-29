using RafaStore.Clientes.API.Application.Commands;
using RafaStore.Core.Mediator;
using RafaStore.Core.Messages.Integration;
using RafaStore.MessageBus;

namespace RafaStore.Clientes.API.Services;

public class RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus) : BackgroundService
{
    private void SetResponder()
    {
        bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request
            => await RegistrarCliente(request));

        bus.AdvancedBus.Connected += OnConnect;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();

        return Task.CompletedTask;
    }

    private void OnConnect(object s, EventArgs e)
    {
        SetResponder();
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
    {
        var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        var sucesso = await mediator.EnviarComando(clienteCommand);
        
        return new ResponseMessage(sucesso);
    }
}