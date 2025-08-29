namespace RafaStore.Carrinho.API.Model;

public class CarrinhoCliente
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }

    public List<CarrinhoItem> Itens { get; set; } = [];

    public CarrinhoCliente()
    {
    }

    public CarrinhoCliente(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
    }
}