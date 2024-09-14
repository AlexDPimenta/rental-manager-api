using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Features.MotorCycles;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

namespace RentalManager.WebApi.Tests.Features.MotorCycles;

public class GetMotorCycleByPlateTestCase
{
    private readonly IMotorCycleRepository _repository = Substitute.For<IMotorCycleRepository>();
    private readonly GetMotorCyclesByPlate.Handler _handler;

    public GetMotorCycleByPlateTestCase()
    {
        _handler = new GetMotorCyclesByPlate.Handler(_repository);
    }

    [Fact]
    public async Task Handle_WhenMotorCycleExists_ReturnsMotorCycleResponse()
    {
        // Arrange
        var request = new GetMotorCyclesByPlate.Query("test-plate");
        _repository.GetMotorCycleByPlateAsync(request.Plate, Arg.Any<CancellationToken>()).Returns(new List<MotorCycle>() { new MotorCycle { Plate = request.Plate } });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.MotorCycles.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_WhenMotorCycleDoesNotExists_ReturnsFailure()
    {
        // Arrange
        var request = new GetMotorCyclesByPlate.Query("test-plate");
        _repository.GetMotorCycleByPlateAsync(request.Plate, Arg.Any<CancellationToken>()).Returns(new List<MotorCycle>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Contain("Moto não encontrada");
    }

}
