using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Persistence.Context;

namespace RentalManager.WebApi.Tests.Mocks;

public class RentalManagerDbContextMock
{
    public static RentalManagerDbContext Create()
    {
        var options = new DbContextOptionsBuilder<RentalManagerDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

        var context = new RentalManagerDbContext(options);

        return context;
    }
}
