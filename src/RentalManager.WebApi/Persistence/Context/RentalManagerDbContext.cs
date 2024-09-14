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
        new DriverMapping(modelBuilder);
        new LeaseMapping(modelBuilder);
    }    

    public DbSet<MotorCycle> MotorCycles { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Lease> Leases { get; set; }
}
