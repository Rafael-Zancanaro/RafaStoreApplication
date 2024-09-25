using Microsoft.EntityFrameworkCore;
using RafaStore.Catalogo.API.Models;
using RafaStore.Core.Data;

namespace RafaStore.Catalogo.API.Data.Repository;

public class ProdutoRepository(CatalogoContext context) : IProdutoRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<IEnumerable<Produto>> ObterTodos()
    {
        return await context.Produtos.AsNoTracking().ToListAsync();
    }

    public async Task<Produto> ObterPorId(Guid id)
    {
        return await context.Produtos.FindAsync(id);
    }

    public void Adicionar(Produto produto)
    {
        context.Produtos.AddAsync(produto);
    }

    public void Atualizar(Produto produto)
    {
        context.Produtos.Update(produto);
    }

    public void Dispose() => context.Dispose();
}