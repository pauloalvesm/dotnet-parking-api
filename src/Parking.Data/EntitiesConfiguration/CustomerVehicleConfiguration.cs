using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Domain.Entities;

namespace Parking.Data.EntitiesConfiguration;

public class CustomerVehicleConfiguration : IEntityTypeConfiguration<CustomerVehicle>
{
    public void Configure(EntityTypeBuilder<CustomerVehicle> builder)
    {
        builder.ToTable("CustomerVehicles");

        builder.HasKey(cv => cv.Id);

        builder.HasOne(cv => cv.Customer)
            .WithMany(c => c.CustomerVehicles)
            .HasForeignKey(cv => cv.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cv => cv.Vehicle)
            .WithMany(v => v.CustomerVehicles)
            .HasForeignKey(cv => cv.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}