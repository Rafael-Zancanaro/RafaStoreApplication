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

        await PersistirDados();
        return CustomResponse();
    }

    [HttpPut("/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();
        var itemCarrinho = await ObterCarrinhoItemValidado(produtoId, carrinho, item);
        if (itemCarrinho == null) return CustomResponse();

        carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

        context.CarrinhoItens.Update(itemCarrinho);
        context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();
        return CustomResponse();
    }

    [HttpDelete("/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var carrinho = await ObterCarrinhoCliente();
        var itemCarrinho = await ObterCarrinhoItemValidado(produtoId, carrinho);
        if(itemCarrinho == null) return CustomResponse();

        carrinho.RemoverItem(itemCarrinho);

        context.CarrinhoItens.Remove(itemCarrinho);
        context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();
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

    private async Task<CarrinhoItem> ObterCarrinhoItemValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem item = null)
    {
        if (item != null && produtoId != item.ProdutoId)
        {
            AdicionarErroProcessamento("O item não corresponde ao informado");
            return null;
        }

        if (carrinho == null)
        {
            AdicionarErroProcessamento("Carrinho não encontrado");
            return null;
        }

        var itemCarrinho = await context.CarrinhoItens.FirstOrDefaultAsync(i => i.CarrinhoId == carrinho.Id && i.ProdutoId == produtoId);

        if (itemCarrinho == null || !carrinho.CarrinhoItemExistente(itemCarrinho))
        {
            AdicionarErroProcessamento("O item não está no carrinho");
            return null;
        }

        return itemCarrinho;
    }

    private async Task PersistirDados()
    {
        var result = await context.SaveChangesAsync();
        if (result <= 0) AdicionarErroProcessamento("Não foi possível persistir os dados no banco");
    }
}