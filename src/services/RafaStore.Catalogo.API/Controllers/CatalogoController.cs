using Microsoft.AspNetCore.Mvc;
using RafaStore.Catalogo.API.Models;

namespace RafaStore.Catalogo.API.Controllers;

[Route("api/catalogo")]
[ApiController]
public class CatalogoController(IProdutoRepository produtoRepository) : Controller
{
    [HttpGet("produtos")]
    public async Task<IEnumerable<Produto>> BuscarTodos()
    {
        return await produtoRepository.ObterTodos();
    }

    [HttpGet("produtos/{id}")]
    public async Task<Produto> Get(Guid id)
    {
        return await produtoRepository.ObterPorId(id);
    }
}
