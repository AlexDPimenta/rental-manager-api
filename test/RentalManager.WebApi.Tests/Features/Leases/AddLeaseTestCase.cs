using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Features.Leases;
using RentalManager.WebApi.Persistence.Context;
using RentalManager.WebApi.Persistence.Repository.LeaseRepository;
using RentalManager.WebApi.Tests.Mocks;

namespace RentalManager.WebApi.Tests.Features.Leases;

public class AddLeaseTestCase
{
    private readonly ILeaseRepository repository = Substitute.For<ILeaseRepository>();
    private readonly RentalManagerDbContext context;
    private readonly AddLease.Handler handler;

    public AddLeaseTestCase()
    {
        context = RentalManagerDbContextMock.Create();
        handler = new AddLease.Handler(repository, context);
    }

    [Fact]
    public async Task ShouldReturnFailureWhenDriverOrPlanOrMotorCycleIsNull()
    {
        // Arrange
        var request = new AddLease.Command("1", "1", 1, DateTime.Now, DateTime.Now, DateTime.Now);                

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("Dados inválidos!");        
    }

    [Fact]
    public async Task ShouldReturnFailureWhenDriverLicenseCategoryDoesNotContainA()
    {
        // Arrange
        var driver = new Driver { 
            Id = "1",  
            LicenseCategory = "B", 
            BirthdayDate = new DateTime(1900,1,1), 
            Cnpj = "123456", 
            LicenseImage = "base64", 
            LicenseNumber = "number123", 
            Name = "name123"
        };
        var motorCycle = new MotorCycle { Id = "1", Model = "Model", Plate = "plate", Year = 2024 };
        var plan = new Plan { DurationInDays = 7, CostPerDay = 15.00m };
        await context.Drivers.AddAsync(driver, CancellationToken.None);
        await context.MotorCycles.AddAsync(motorCycle, CancellationToken.None);
        await context.Plans.AddAsync(plan, CancellationToken.None);
        await context.SaveChangesAsync();

        var request = new AddLease.Command("1", "1", 7, DateTime.Now, DateTime.Now, DateTime.Now);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task ShouldReturnFailureWhenStartDateIsGreaterThanEndDate()
    {
        // Arrange
        var driver = new Driver
        {
            Id = "1",
            LicenseCategory = "A",
            BirthdayDate = new DateTime(1900, 1, 1),
            Cnpj = "123456",
            LicenseImage = "base64",
            LicenseNumber = "number123",
            Name = "name123"
        };
        var motorCycle = new MotorCycle { Id = "1", Model = "Model", Plate = "plate", Year = 2024 };
        var plan = new Plan { DurationInDays = 7, CostPerDay = 15.00m };
        await context.Drivers.AddAsync(driver, CancellationToken.None);
        await context.MotorCycles.AddAsync(motorCycle, CancellationToken.None);
        await context.Plans.AddAsync(plan, CancellationToken.None);
        await context.SaveChangesAsync();

        var request = new AddLease.Command("1", "1", 7, DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task ShouldReturnFailureWhenStartDateIsGreaterThanExpectedEndDate()
    {
        // Arrange
        var driver = new Driver
        {
            Id = "1",
            LicenseCategory = "A",
            BirthdayDate = new DateTime(1900, 1, 1),
            Cnpj = "123456",
            LicenseImage = "base64",
            LicenseNumber = "number123",
            Name = "name123"
        };
        var motorCycle = new MotorCycle { Id = "1", Model = "Model", Plate = "plate", Year = 2024 };
        var plan = new Plan { DurationInDays = 7, CostPerDay = 15.00m };
        await context.Drivers.AddAsync(driver, CancellationToken.None);
        await context.MotorCycles.AddAsync(motorCycle, CancellationToken.None);
        await context.Plans.AddAsync(plan, CancellationToken.None);
        await context.SaveChangesAsync();

        var request = new AddLease.Command("1", "1", 7, DateTime.Now, DateTime.Now, DateTime.Now.AddDays(-1));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task ShouldReturnSuccessWhenAllDataIsValid()
    {
        // Arrange
        var driver = new Driver
        {
            Id = "1",
            LicenseCategory = "A",
            BirthdayDate = new DateTime(1900, 1, 1),
            Cnpj = "123456",
            LicenseImage = "base64",
            LicenseNumber = "number123",
            Name = "name123"
        };
        var motorCycle = new MotorCycle { Id = "1", Model = "Model", Plate = "plate", Year = 2024 };
        var plan = new Plan { DurationInDays = 7, CostPerDay = 15.00m };
        await context.Drivers.AddAsync(driver, CancellationToken.None);
        await context.MotorCycles.AddAsync(motorCycle, CancellationToken.None);
        await context.Plans.AddAsync(plan, CancellationToken.None);
        await context.SaveChangesAsync();

        var request = new AddLease.Command("1", "1", 7, DateTime.Now, DateTime.Now, DateTime.Now);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
