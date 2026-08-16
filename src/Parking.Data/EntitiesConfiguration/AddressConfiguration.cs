using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Domain.Entities;

namespace Parking.Data.EntitiesConfiguration;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Complement)
            .HasMaxLength(150);

        builder.Property(a => a.Neighborhood)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.FederativeUnit)
            .IsRequired()
            .HasMaxLength(2)
            .IsFixedLength();

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ZipCode)
            .IsRequired()
            .HasMaxLength(9);
    }
}