namespace RafaStore.Carrinho.API.Model;

public class CarrinhoItem
{
	public CarrinhoItem()
	{
		Id = Guid.NewGuid();
	}

	public Guid Id { get; set; }
	public Guid ProdutoId { get; set; }
	public string Name { get; set; }
	public int Quantidade { get; set; }
	public decimal Valor { get; set; }
	public string Imagem { get; set; }

	public CarrinhoCliente CarrinhoCliente { get; set; }
	public Guid CarrinhoId { get; set; }
}
