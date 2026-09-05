using Microsoft.EntityFrameworkCore;
using Parking.Data.Context;

namespace Parking.Data.Test.Context.Helpers;

public class FaultyDbContext : ApplicationDbContext
{
    public FaultyDbContext() : base(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options) {}

    public override DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        throw new InvalidOperationException("Simulated database failure");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Simulated database failure");
    }
}