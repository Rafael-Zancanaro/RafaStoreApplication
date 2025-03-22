using FluentValidation.Results;
using MediatR;
using RafaStore.Clientes.API.Application.Events;
using RafaStore.Clientes.API.Models;
using RafaStore.Core.Messages;

namespace RafaStore.Clientes.API.Application.Commands;

public class ClienteCommandHandler(IClienteRepository clienteRepository) : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
{
    public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido()) return message.ValidationResult;

        var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);
        
        var clienteExistente = await clienteRepository.ObterPorCpf(cliente.Cpf.Numero);
        if (clienteExistente != null)
        {
            AdicionarErro("Este CPF já está em uso.");
            return ValidationResult;
        }
        
        clienteRepository.Adicionar(cliente);
        
        cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

        return await PersistirDados(clienteRepository.UnitOfWork);
    }
}