using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Mappings;

public class MotorCycleMapping
{
    public MotorCycleMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MotorCycle>(entity =>
        {
            entity.ToTable("motorcycles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Model).HasColumnName("model").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Plate).HasColumnName("plate").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        });
    }
}
