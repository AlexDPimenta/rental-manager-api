using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Persistence.Context;

namespace RentalManager.WebApi.Persistence.Repository.DriversRepository;

public class DriverRepository(RentalManagerDbContext dbContext): IDriverRepository
{
    public async Task<Driver> AddDriverAsync(Driver driver, CancellationToken cancellationToken)
    {
        await dbContext.Drivers.AddAsync(driver, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return driver;
    }

    public async Task<Driver> GetDriverByCnpjOrLicenseNumber(string cnpj, string licenseNumber, CancellationToken cancellationToken)
    {
        var driver = await dbContext.Drivers.FirstOrDefaultAsync(d => d.Cnpj == cnpj || d.LicenseNumber == licenseNumber, cancellationToken);
        return driver;
    }

    public Task UpdateDriverLicenseImage(string licenseImage, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
