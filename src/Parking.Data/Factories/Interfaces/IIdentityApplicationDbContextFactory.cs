using Parking.Data.Context;

namespace Parking.Data.Factories.Interfaces;

public interface IIdentityApplicationDbContextFactory
{
    IdentityApplicationDbContext CreateDbContext();
}
