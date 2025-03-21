using System.Text.RegularExpressions;

namespace RafaStore.Core.DomainObjects;

public partial class Email
{
    public const int EnderecoMaxLength = 254;
    public const int EnderecoMinLength = 5;
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