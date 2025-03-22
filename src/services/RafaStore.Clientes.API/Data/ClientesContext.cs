using Microsoft.EntityFrameworkCore;
using RafaStore.Clientes.API.Models;
using RafaStore.Core.Data;
using RafaStore.Core.DomainObjects;
using RafaStore.Core.Mediator;

namespace RafaStore.Clientes.API.Data;

public sealed class ClientesContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;
    public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediatorHandler) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
        _mediatorHandler = mediatorHandler;
    }
    
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
        
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");
        
        foreach (var relationShip in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;
    }

    public async Task<bool> CommitAsync()
    {
        var sucesso = await SaveChangesAsync() > 0;
        if (sucesso)
            await _mediatorHandler.PublicarEventos(this);
        
        return sucesso;
    }
}

public static class MediatorExtension
{
    public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Count != 0);
        
        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notifications)
            .ToList();
        
        domainEntities.ToList()
            .ForEach(x => x.Entity.LimparEventos());

        var tasks = domainEvents.Select(async (domainEvent) => await mediator.PublicarEvento(domainEvent));
        
        await Task.WhenAll(tasks);
    } 
}