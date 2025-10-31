using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RafaStore.Carrinho.API.Data;
using RafaStore.Carrinho.API.Model;
using RafaStore.WebAPI.Core.Controllers;
using RafaStore.WebAPI.Core.Usuario;

namespace RafaStore.Carrinho.API.Controllers;

[Route("carrinho")]
[ApiController]
[Authorize]
public class CarrinhoController(IAspNetUser user, CarrinhoContext context) : MainController
{
    [HttpGet]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return await ObterCarrinhoCliente() ?? new();
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();
        if (carrinho == null)
            ManipularNovoCarrinho(item);
        else
            ManipularCarrinhoExistente(carrinho, item);

        if (!OperacaoValida()) return CustomResponse();

        var result = await context.SaveChangesAsync();
        if (result <= 0) AdicionarErroProcessamento("Não foi possivel persistir os dados no banco");
        
        return CustomResponse();
    }

    [HttpPut("/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        return CustomResponse();
    }

    [HttpDelete("/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        return CustomResponse();
    }

    private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
    {
        var produtoItemExistente = carrinho.CarrinhoItemExistente(item);

        carrinho.AdicionarItem(item);

        if (produtoItemExistente)
            context.CarrinhoItens.Update(carrinho.ObterPorProdutoId(item.ProdutoId));
        else
            context.CarrinhoItens.Add(item);

        context.CarrinhoCliente.Update(carrinho);
    }

    private void ManipularNovoCarrinho(CarrinhoItem item)
    {
        var carrinho = new CarrinhoCliente(user.ObterUserId());
        carrinho.AdicionarItem(item);

        context.CarrinhoCliente.Add(carrinho);
    }

    private async Task<CarrinhoCliente> ObterCarrinhoCliente()
    {
        return await context.CarrinhoCliente
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == user.ObterUserId());
    }
}