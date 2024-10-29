using Microsoft.AspNetCore.Mvc;
using RafaStore.WebApp.MVC.Services;

namespace RafaStore.WebApp.MVC.Controllers;
public class CatalogoController(ICatalogoService catalogoService) : MainController
{
    [Route("")]
    [HttpGet("vitrine")]
    public async Task<IActionResult> Index()
    {
        var produtos = await catalogoService.ObterTodos();
        return View(produtos);
    }

    [HttpGet($"produto-detalhe/{{id}}")]
    public async Task<IActionResult> ProdutoDetalhe(Guid id)
    {
        var produto = await catalogoService.ObterPorId(id);
        return View(produto);
    }
}
