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
            entity.Property(e => e.StartDate)
                .IsRequired();
            entity.Property(e => e.EndDate)
                .IsRequired();
            entity.Property(e => e.ExpectedEndDate)
                .IsRequired();           
            
            entity.HasOne(l => l.LeasePlan)   
              .WithMany()
              .HasForeignKey(l => l.DurationInDays);
        });
    }
}
