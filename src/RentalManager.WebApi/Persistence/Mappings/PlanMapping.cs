using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Mappings;

public class PlanMapping
{
    public PlanMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("Plans");
            entity.HasKey(e => e.DurationInDays);
            entity.Property(e => e.CostPerDay).IsRequired().HasColumnType("decimal(10, 2)");

            entity.HasData(
                new Plan { DurationInDays = 7, CostPerDay = 30.00m },
                new Plan { DurationInDays = 15, CostPerDay = 28.00m },
                new Plan { DurationInDays = 30, CostPerDay = 22.00m },
                new Plan { DurationInDays = 45, CostPerDay = 20.00m },
                new Plan { DurationInDays = 50, CostPerDay = 18.00m }
            );
        });
    }
}
