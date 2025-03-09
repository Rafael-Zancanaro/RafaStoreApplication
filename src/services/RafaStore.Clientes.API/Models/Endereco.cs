using RafaStore.Core.DomainObjects;

namespace RafaStore.Cliente.API.Models;

public class Endereco : Entity
{
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cep { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public Guid ClienteId { get; private set; }
    
    public Cliente Cliente { get; private set; }

    public Endereco(string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string estado)
    {
        logradouro = logradouro;
        numero = numero;
        complemento = complemento;
        bairro = bairro;
        cep = cep;
        cidade = cidade;
        estado = estado;
    }
}