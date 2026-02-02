using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafaStore.WebApp.MVC.Models;

namespace RafaStore.WebApp.MVC.Controllers;

[Authorize]
[ApiController]
[Route("carrinho")]
public class CarrinhoController : MainController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost("adicionar-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemProdutoViewModel itemProduto)
    {
        return RedirectToAction("Index");
    }

    [HttpPost("atualizar-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(Guid produtoId, int quantidade)
    {
        return RedirectToAction("Index");
    }

    [HttpPost("remover-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(Guid produtoId)
    {
        return RedirectToAction("Index");
    }
}
