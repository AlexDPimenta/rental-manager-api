using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Mappings;

public class DriverMapping
{
    public DriverMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.ToTable("Drivers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.Name)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.Cnpj)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.BirthdayDate)                
                .IsRequired();
            entity.Property(e => e.LicenseNumber)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.LicenseCategory)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.LicenseImage)
                .HasColumnType("varchar(1500)")                
                .IsRequired();

            entity.HasMany(e => e.Leases)
                .WithOne(c => c.Driver)
                .HasForeignKey(c => c.DriverId);                
        });
    }
}
