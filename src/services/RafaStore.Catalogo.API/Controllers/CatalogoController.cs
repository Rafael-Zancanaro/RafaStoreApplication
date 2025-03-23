using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafaStore.Catalogo.API.Models;
using RafaStore.WebAPI.Core.Controllers;
using RafaStore.WebAPI.Core.Identidade;

namespace RafaStore.Catalogo.API.Controllers;

[Route("api/catalogo")]
[ApiController]
[Authorize]
public class CatalogoController(IProdutoRepository produtoRepository) : MainController
{
    [AllowAnonymous]
    [HttpGet("produtos")]
    public async Task<IEnumerable<Produto>> BuscarTodos()
    {
        return await produtoRepository.ObterTodos();
    }

    [ClaimsAuthorize("Catalogo","Ler")]
    [HttpGet("produtos/{id:guid}")]
    public async Task<Produto> Get(Guid id)
    {
        return await produtoRepository.ObterPorId(id);
    }
}