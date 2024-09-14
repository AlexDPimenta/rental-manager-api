using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Mappings;

public class MotorCycleMapping
{
    public MotorCycleMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MotorCycle>(entity =>
        {
            entity.ToTable("MotorCycles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)               
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.Year);                
            entity.Property(e => e.Model)               
                .HasColumnType("varchar(100)")
                .HasMaxLength(100).IsRequired();
            entity.Property(e => e.Plate)                
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            entity.HasMany(e => e.Leases)
                .WithOne(l => l.MotorCycle)
                .HasForeignKey(c => c.MotorCycleId)
                .OnDelete(DeleteBehavior.Cascade);                
        });
    }
}
