using FluentValidation;

namespace RafaStore.Carrinho.API.Model;

public class CarrinhoItem
{
    public Guid Id { get; set; }
	public Guid ProdutoId { get; set; }
	public string Name { get; set; }
	public int Quantidade { get; set; }
	public decimal Valor { get; set; }
	public string Imagem { get; set; }

	public CarrinhoCliente CarrinhoCliente { get; set; }
	public Guid CarrinhoId { get; set; }

    public CarrinhoItem()
    {
        Id = Guid.NewGuid();
    }

	internal void AssociarCarrinho(Guid carrinhoId)
	{
		CarrinhoId = carrinhoId;
	}

	internal decimal CalcularValor()
	{
		return Quantidade * Valor;
	}

	internal void AdicionarUnidades(int unidades)
	{
		Quantidade += unidades;
	}

	internal bool EhValido()
	{
		return new ItemPedidoValidation().Validate(this).IsValid;
	}

	public class ItemPedidoValidation : AbstractValidator<CarrinhoItem>
	{
		public ItemPedidoValidation()
		{
			RuleFor(c => c.ProdutoId)
				.NotEqual(Guid.Empty)
				.WithMessage("Id do produto inválido");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("O nome do produto não foi informado");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage("A quantidade minima de um item é 1");

            RuleFor(c => c.Quantidade)
                .LessThanOrEqualTo(5)
                .WithMessage("A quantidade máxima de um item é 5");

            RuleFor(c => c.Valor)
                .GreaterThan(0)
                .WithMessage("O valor do item precisa ser maior que 0");
        }
	}
}