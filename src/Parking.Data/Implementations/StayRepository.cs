using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parking.Data.Context;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Implementations;

public class StayRepository : Repository<Stay>, IStayRepository
{
    public StayRepository(ApplicationDbContext context, ILogger<Stay> logger) : base(context, logger) { }

    public override async Task<IEnumerable<Stay>> GetAllAsync()
    {
        try
        {
            var stays = await _context.Stays
                .AsNoTracking()
                .Include(s => s.CustomerVehicle)
                    .ThenInclude(cv => cv.Customer)
                .Include(s => s.CustomerVehicle)
                    .ThenInclude(cv => cv.Vehicle)
                .ToListAsync();

            return stays;
        }
        catch (Exception exception)
        {
            var errorMessage = $"Error when searching list of records: {exception.Message}";
            _logger.LogError(exception, errorMessage);
            throw;
        }
    }

    public override async Task<Stay> GetByIdAsync(int id)
    {
        try
        {
            var stay = await _context.Stays
                .Include(s => s.CustomerVehicle)
                    .ThenInclude(cv => cv.Customer)
                .Include(s => s.CustomerVehicle)
                    .ThenInclude(cv => cv.Vehicle)
                .FirstOrDefaultAsync(s => s.Id == id);

            return stay;
        }
        catch (Exception exception)
        {
            var errorMessage = $"Error getting record with ID: {id}. Message: {exception.Message}";
            _logger.LogError(exception, errorMessage);
            throw;
        }
    }

    public override async Task<Stay> UpdateAsync(Stay stay)
    {
        try
        {
            _context.Stays.Update(stay);
            await _context.SaveChangesAsync();
            return stay;
        }
        catch (Exception exception)
        {
            var errorMessage = $"Error when updating the record: {exception.Message}";
            _logger.LogError(exception, errorMessage);
            throw;
        }
    }
}