using RafaStore.WebApp.MVC.Models;
using Refit;

namespace RafaStore.WebApp.MVC.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    Task<ProdutoViewModel> ObterPorId(Guid id);
}

public interface ICatalogoServiceRefit
{
    [Get("/api/catalogo/produtos/")]
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    
    [Get("/api/catalogo/produtos/{id}")]
    Task<ProdutoViewModel> ObterPorId(Guid id);
}