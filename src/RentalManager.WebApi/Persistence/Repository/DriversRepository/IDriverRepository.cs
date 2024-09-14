using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Repository.DriversRepository;

public interface IDriverRepository
{
    Task<Driver> AddDriverAsync(Driver driver, CancellationToken cancellationToken);
    Task UpdateDriverLicenseImage(string licenseImage, CancellationToken cancellationToken);
    Task<Driver> GetDriverByCnpjOrLicenseNumber(string cnpj, string licenseNumber, CancellationToken cancellationToken);
}
