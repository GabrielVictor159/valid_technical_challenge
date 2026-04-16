using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using System.Reflection;

namespace Store.Infraestructure.Data.Contexts;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Operation> Operations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedDate") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
