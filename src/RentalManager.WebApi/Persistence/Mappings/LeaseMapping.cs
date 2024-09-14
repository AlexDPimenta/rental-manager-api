using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Mappings;

public class LeaseMapping
{
    public LeaseMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lease>(entity =>
        {
            entity.ToTable("Leases");
            entity.HasKey(e => e.Id);            
            entity.Property(e => e.MotorCycleId)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired();
            entity.Property(e => e.DriverId)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired();
            entity.Property(e => e.StartDate)
                .IsRequired();
            entity.Property(e => e.EndDate)
                .IsRequired();
            entity.Property(e => e.ExpectedEndDate)
                .IsRequired();
            entity.HasOne(e => e.MotorCycle)
                .WithMany()
                .HasForeignKey(e => e.MotorCycleId);
            entity.HasOne(e => e.Driver)
                .WithMany()
                .HasForeignKey(e => e.DriverId);
        });
    }
}
