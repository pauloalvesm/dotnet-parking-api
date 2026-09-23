using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parking.Data.Context;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Implementations;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context, ILogger<Customer> logger) : base(context, logger) {}

    public override async Task<IEnumerable<Customer>> GetAllAsync()
    {
        try
        {
            return await _context.Set<Customer>()
                .Include(c => c.Address)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error when searching list of records: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }

    public override async Task<Customer> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Set<Customer>()
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error getting record with ID: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }
}
