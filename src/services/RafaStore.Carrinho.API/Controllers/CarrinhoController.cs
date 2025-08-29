using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafaStore.Carrinho.API.Model;
using RafaStore.WebAPI.Core.Controllers;
using RafaStore.WebAPI.Core.Usuario;

namespace RafaStore.Carrinho.API.Controllers;

[Route("carrinho")]
[ApiController]
[Authorize]
public class CarrinhoController(IAspNetUser user) : MainController
{
    [HttpGet]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return null;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarItemTemporario(CarrinhoItem item)
    {
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
}