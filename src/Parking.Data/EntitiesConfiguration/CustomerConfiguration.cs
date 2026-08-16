using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Domain.Entities;

namespace Parking.Data.EntitiesConfiguration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.BirthDate);

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(11)
            .IsFixedLength();

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(c => c.Address)
            .WithMany(a => a.Customers)
            .HasForeignKey(c => c.AddressId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}