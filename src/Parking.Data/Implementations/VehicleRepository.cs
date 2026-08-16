using Microsoft.Extensions.Logging;
using Parking.Data.Context;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Implementations;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(ApplicationDbContext context, ILogger<Vehicle> logger) : base(context, logger) {}
}
