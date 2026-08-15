using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parking.Data.Context;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Implementations;

public class CustomerVehicleRepository : Repository<CustomerVehicle>, ICustomerVehicleRepository
{
    public CustomerVehicleRepository(ApplicationDbContext context, ILogger<CustomerVehicle> logger) : base(context, logger) {}

    public override async Task<IEnumerable<CustomerVehicle>> GetAllAsync()
    {
        try
        {
            var details = await _context.CustomerVehicles
                .Include(cv => cv.Customer)
                .Include(cv => cv.Vehicle)
                .ToListAsync();

            foreach (var detail in details)
            {
                var customerName = detail.Customer.Name;
                var vehicleName = detail.Vehicle.Model;
            }

            return details;
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error when searching list of records: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }
}
