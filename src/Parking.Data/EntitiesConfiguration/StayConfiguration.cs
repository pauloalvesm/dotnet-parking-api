using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Domain.Entities;

namespace Parking.Data.EntitiesConfiguration;

public class StayConfiguration : IEntityTypeConfiguration<Stay>
{
    public void Configure(EntityTypeBuilder<Stay> builder)
    {
        builder.ToTable("Stays");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.LicensePlate)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(s => s.EntryDate);

        builder.Property(s => s.ExitDate);

        builder.Property(s => s.HourlyRate)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(s => s.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(s => s.StayStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.HasOne(s => s.CustomerVehicle)
            .WithMany(cv => cv.Stays)
            .HasForeignKey(s => s.CustomerVehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}