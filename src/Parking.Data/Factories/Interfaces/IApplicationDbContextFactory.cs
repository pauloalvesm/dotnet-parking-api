using Parking.Data.Context;

namespace Parking.Data.Factories.Interfaces;

public interface IApplicationDbContextFactory
{
    ApplicationDbContext CreateDbContext();
}
