using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parking.Data.Context;
using Parking.Domain.Common;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Implementations;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext _context;
    protected readonly ILogger<TEntity> _logger;
    protected string _errorMessage = "";

    public Repository(ApplicationDbContext context, ILogger<TEntity> logger)
    {
        _context = context;
        _logger = logger;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error when searching list of records: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }

    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error getting record with ID: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error when adding a new record: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error when updating the record: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }

    public virtual async Task DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception exception)
        {
            _errorMessage = $"Error when deleting the record: {exception.Message}";
            _logger.LogError(exception, _errorMessage);
            throw;
        }
    }
}
