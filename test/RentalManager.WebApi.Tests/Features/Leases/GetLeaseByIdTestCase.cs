using RentalManager.WebApi.Contracts.Leases;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Persistence.Repository.LeaseRepository;
using RentalManager.WebApi.Features.Leases;

namespace RentalManager.WebApi.Tests.Features.Leases;

public class GetLeaseByIdTestCase
{
    private readonly ILeaseRepository repository = Substitute.For<ILeaseRepository>();
    private readonly GetLeaseById.Handler handler;

    public GetLeaseByIdTestCase()
    {
        handler = new GetLeaseById.Handler(repository);
    }

    [Fact]
    public async Task ShouldReturnFailureWhenLeaseIsNull()
    {
        // Arrange
        var request = new GetLeaseById.Query("1");
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
            ExpectedEndDate = DateTime.Now,
            LeasePlan = new Plan { CostPerDay = 15.00m }
        };
        repository.GetLeaseByIdAsync("1", CancellationToken.None).Returns(lease);

        var request = new GetLeaseById.Query("1");

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be("1");
        result.Value.CostPerDay.Should().Be(15.00m);
    }
}
