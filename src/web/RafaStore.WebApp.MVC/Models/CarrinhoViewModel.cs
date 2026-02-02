namespace RafaStore.WebApp.MVC.Models;

public class CarrinhoViewModel
{
    public decimal ValorTotala { get; set; }
    public List<ItemProdutoViewModel> Itens { get; set; }
}

public class ItemProdutoViewModel
{
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorTotal { get; set; }
    public string Imagem { get; set; }
}
