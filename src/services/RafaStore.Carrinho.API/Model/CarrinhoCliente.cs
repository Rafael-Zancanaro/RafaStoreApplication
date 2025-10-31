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

    internal void CalcularValorCarrinho()
    {
        ValorTotal = Itens.Sum(x => x.CalcularValor());
    }

    internal bool CarrinhoItemExistente(CarrinhoItem item)
    {
        return Itens.Any(x => x.ProdutoId == item.ProdutoId);
    }

    internal CarrinhoItem ObterPorProdutoId(Guid produtoId)
    {
        return Itens.FirstOrDefault(x => x.ProdutoId == produtoId);
    }

    internal void AdicionarItem(CarrinhoItem item)
    {
        if (!item.EhValido()) return;

        item.AssociarCarrinho(Id);

        if (CarrinhoItemExistente(item))
        {
            var itemExistente = ObterPorProdutoId(item.ProdutoId);
            itemExistente.AdicionarUnidades(item.Quantidade);

            item = itemExistente;
            Itens.Remove(itemExistente);
        }

        Itens.Add(item);
        CalcularValorCarrinho();
    }
}