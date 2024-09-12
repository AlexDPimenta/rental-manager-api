using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Persistence.Mappings;

namespace RentalManager.WebApi.Persistence.Context;

public sealed class RentalManagerDbContext: DbContext
{
    public RentalManagerDbContext(DbContextOptions<RentalManagerDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new MotorCycleMapping(modelBuilder);
    }    

    public DbSet<MotorCycle> MotorCycles { get; set; }
}
