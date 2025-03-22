using Microsoft.EntityFrameworkCore;
using RafaStore.Clientes.API.Models;
using RafaStore.Core.Data;

namespace RafaStore.Clientes.API.Data.Repository;

public class ClienteRepository(ClientesContext context) : IClienteRepository
{
    public IUnitOfWork UnitOfWork => context;

    public void Adicionar(Cliente cliente)
    {
        context.Clientes.Add(cliente);
    }

    public async Task<IEnumerable<Cliente>> ObterTodos()
        => await context.Clientes.AsNoTracking().ToListAsync();

    public async Task<Cliente> ObterPorCpf(string cpf)
        => await context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
    
    public void Dispose()
    {
        context.Dispose();
    }
}