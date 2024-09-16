using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Repository.LeaseRepository;

public interface ILeaseRepository
{
    Task<Lease> AddLeaseAsync(Lease lease, CancellationToken cancellationToken);

    Task<Lease> GetLeaseByIdAsync(string id, CancellationToken cancellationToken);

}
