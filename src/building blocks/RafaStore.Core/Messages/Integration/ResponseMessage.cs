using FluentValidation.Results;

namespace RafaStore.Core.Messages.Integration;

public class ResponseMessage(ValidationResult validationResult) : Message
{
    public ValidationResult ValidationResult { get; set; } = validationResult;
}