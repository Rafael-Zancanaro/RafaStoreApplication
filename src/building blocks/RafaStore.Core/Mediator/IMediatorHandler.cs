using FluentValidation.Results;
using RafaStore.Core.Messages;

namespace RafaStore.Core.Mediator;

public interface IMediatorHandler
{
    Task PublicarEvento<T>(T evento) where T : Event;
    Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
}