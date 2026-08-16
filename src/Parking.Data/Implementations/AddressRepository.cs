using Microsoft.Extensions.Logging;
using Parking.Data.Context;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Implementations;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context, ILogger<Address> logger) : base(context, logger) {}
}
