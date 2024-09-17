using RentalManager.WebApi.Contracts.Leases;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Persistence.Repository.LeaseRepository;
using RentalManager.WebApi.Features.Leases;

namespace RentalManager.WebApi.Tests.Features.Leases;

public class UpdateLeaseByIdTestCase
{
    private readonly ILeaseRepository repository = Substitute.For<ILeaseRepository>();
    private readonly UpdateLeaseById.Handler handler;

    public UpdateLeaseByIdTestCase()
    {
        handler = new UpdateLeaseById.Handler(repository);
    }

    [Fact]
    public async Task ShouldReturnFailureWhenLeaseIsNull()
    {
        // Arrange
        var request = new UpdateLeaseById.Command("1", DateTime.Now);
        repository.GetLeaseByIdAsync("1", CancellationToken.None).Returns((Lease)null);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task ShouldReturnSuccessWhenLeaseIsNotNull()
    {
        // Arrange
        var lease = new Lease
        {
            Id = "1",
            DriverId = "1",
            MotorCycleId = "1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
            DurationInDays = 7,
            ExpectedEndDate = DateTime.Now,
            LeasePlan = new Plan { CostPerDay = 15.00m }
        };
        repository.GetLeaseByIdAsync("1", CancellationToken.None).Returns(lease);

        var request = new UpdateLeaseById.Command("1", DateTime.Now);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCost.Should().Be(105);
    }

    [Fact]
    public async Task ShouldReturnSuccessWhenLeaseIsNotNullAndReturnDataIsGreaterThanExpectedEndDate()
    {
        // Arrange
        var lease = new Lease
        {
            Id = "1",
            DriverId = "1",
            MotorCycleId = "1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
            DurationInDays = 7,
            ExpectedEndDate = DateTime.Now,
            LeasePlan = new Plan { CostPerDay = 15.00m }
        };
        repository.GetLeaseByIdAsync("1", CancellationToken.None).Returns(lease);

        var request = new UpdateLeaseById.Command("1", DateTime.Now.AddDays(10));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCost.Should().Be(755);
    }

    [Fact]
    public async Task ShouldReturnSuccessWhenLeaseIsNotNullAndReturnDataIsEarlierThanExpectedEndDate()
    {
        // Arrange
        var lease = new Lease
        {
            Id = "1",
            DriverId = "1",
            MotorCycleId = "1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7),
            DurationInDays = 7,
            ExpectedEndDate = DateTime.Now.AddDays(7),
            LeasePlan = new Plan { CostPerDay = 30.00m }
        };
        repository.GetLeaseByIdAsync("1", CancellationToken.None).Returns(lease);

        var request = new UpdateLeaseById.Command("1", DateTime.Now.AddDays(3));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCost.Should().Be(84);
    }


}
