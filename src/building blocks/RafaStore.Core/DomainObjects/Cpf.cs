namespace RafaStore.Core.DomainObjects;

public class Cpf
{
    public const int CpfMaxLength = 11;
    public string Numero { get; private set; }
    
    protected Cpf() { }

    public Cpf(string numero)
    {
        if (!Validar(numero)) throw new DomainException("Cpf inválido");
        Numero = numero;
    }

    public static bool Validar(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != CpfMaxLength) return false;

        if (cpf.Distinct().Count() == 1) return false;

        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        string tempCpf = cpf[..9];
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += (tempCpf[i] - '0') * multiplicador1[i];

        int resto = soma % 11;
        int primeiroDigito = resto < 2 ? 0 : 11 - resto;

        tempCpf += primeiroDigito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += (tempCpf[i] - '0') * multiplicador2[i];

        resto = soma % 11;
        int segundoDigito = resto < 2 ? 0 : 11 - resto;

        return cpf[CpfMaxLength - 2] - '0' == primeiroDigito && cpf[CpfMaxLength - 1] - '0' == segundoDigito;
    }
}