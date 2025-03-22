using FluentValidation.Results;
using RafaStore.Core.Data;

namespace RafaStore.Core.Messages;

public class CommandHandler
{
    protected ValidationResult ValidationResult;

    protected CommandHandler()
    {
        ValidationResult = new ValidationResult();
    }

    public void AdicionarErro(string mensagem)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
    }

    protected async Task<ValidationResult> PersistirDados(IUnitOfWork unitOfWork)
    {
        if (!await unitOfWork.CommitAsync())
            AdicionarErro("Houve um erro ao persistir os dados");
        
        return ValidationResult;
    }
}