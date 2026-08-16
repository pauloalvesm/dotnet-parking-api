using Microsoft.EntityFrameworkCore;
using Parking.Domain.Entities;

namespace Parking.Data.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Address> Addresses { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<CustomerVehicle> CustomerVehicles { get; set; }

    public DbSet<Stay> Stays { get; set; }

    public ApplicationDbContext() {}

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
