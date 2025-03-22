using FluentValidation.Results;
using MediatR;

namespace RafaStore.Core.Messages;

public abstract class Command : Message, IRequest<ValidationResult>
{
    public DateTime Timestamp { get; private set; } = DateTime.Now;
    public ValidationResult ValidationResult { get; set; }

    public virtual bool EhValido()
    {
        throw new NotImplementedException();
    }
}