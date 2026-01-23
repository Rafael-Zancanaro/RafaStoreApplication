using FluentValidation;
using FluentValidation.Results;

namespace RafaStore.Carrinho.API.Model;

public class CarrinhoCliente
{
    public const int QuantidadeMaximaPermitida = 5;
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public ValidationResult ValidationResult { get; set; }

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

    internal void AtualizarItem(CarrinhoItem item)
    {
        item.AssociarCarrinho(Id);

        var itemExistente = ObterPorProdutoId(item.ProdutoId);

        Itens.Remove(itemExistente);
        Itens.Add(item);

        CalcularValorCarrinho();
    }

    internal void AtualizarUnidades(CarrinhoItem item, int unidades)
    {
        item.AtualizarUnidades(unidades);
        AtualizarItem(item);
    }

    internal void RemoverItem(CarrinhoItem item)
    {
        Itens.Remove(ObterPorProdutoId(item.ProdutoId));

        CalcularValorCarrinho();
    }

    internal bool EhValido()
    {
        var erros = Itens.SelectMany(i => new CarrinhoItem.ItemCarrinhoValidation().Validate(i).Errors).ToList();
        erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);
        ValidationResult= new ValidationResult(erros);

        return ValidationResult.IsValid;
    }

    public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
    {
        public CarrinhoClienteValidation()
        {
            RuleFor(x => x.ClienteId)
                .NotEmpty()
                .WithMessage("Cliente não reconhecido");

            RuleFor(x => x.Itens.Count)
                .GreaterThan(0)
                .WithMessage("O carrinho não possui itens");

            RuleFor(x => x.ValorTotal)
                .GreaterThan(0)
                .WithMessage("O valor total precisa ser maior que 0");
        }
    }
}