using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Persistence.Context;

namespace RentalManager.WebApi.Persistence.Repository.LeaseRepository;

public class LeaseRepository (RentalManagerDbContext context) : ILeaseRepository
{
    public async Task<Lease> AddLeaseAsync(Lease lease, CancellationToken cancellationToken)
    {
        await context.Leases.AddAsync(lease, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return lease;
    }

    public async Task<Lease> GetLeaseByIdAsync(string id, CancellationToken cancellationToken)
    {
        var lease = await context.Leases
            .Include(l => l.Driver)
            .Include(l => l.MotorCycle)
            .Include(l => l.LeasePlan) 
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return lease;
    }
}
