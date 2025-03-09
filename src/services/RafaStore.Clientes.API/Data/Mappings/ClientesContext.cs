using Microsoft.EntityFrameworkCore;
using RafaStore.Core.Data;

namespace RafaStore.Cliente.API.Data.Mappings;

public class ClientesContext : DbContext, IUnitOfWork
{
    public ClientesContext(DbContextOptions<ClientesContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }
}