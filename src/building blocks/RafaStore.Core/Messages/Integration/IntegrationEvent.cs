namespace RafaStore.Core.Messages.Integration;

public abstract class IntegrationEvent : Event
{
    
}

public class UsuarioRegistradoIntegrationEvent(Guid id, string nome, string email, string cpf)
    : Event
{
    public Guid Id { get; private set; } = id;
    public string Nome { get; private set; } = nome;
    public string Email { get; private set; } = email;
    public string Cpf { get; private set; } = cpf;
}