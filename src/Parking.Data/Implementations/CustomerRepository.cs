using Microsoft.Extensions.Logging;
using Parking.Data.Context;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Implementations;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context, ILogger<Customer> logger) : base(context, logger) {}
}
