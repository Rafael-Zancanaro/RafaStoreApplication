using System.Text.RegularExpressions;

namespace RafaStore.Core.DomainObjects;

public class Email
{
    private const int EnderecoMaxLength = 254;
    private const int EnderecoMinLength = 5;
    public string Endereco { get; private set; }
    
    protected Email() { }

    public Email(string endereco)
    {
        if (!Validar(endereco)) throw new DomainException("E-mail inválido");
        Endereco = endereco;
    }

    public static bool Validar(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco) || endereco.Length < EnderecoMinLength || endereco.Length > EnderecoMaxLength)
            return false;
        
        return EmailValidator().IsMatch(endereco);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)]
    private static partial Regex EmailValidator();
}