using RentalManager.WebApi.Common;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Features.MotorCycles;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

namespace RentalManager.WebApi.Tests.Features.MotorCycles;

public class GetMotorCycleByIdTestCase
{
    private readonly IMotorCycleRepository _repository = Substitute.For<IMotorCycleRepository>();
    private readonly GetMotorCycleById.Handler _handler;
    public GetMotorCycleByIdTestCase()
    {
        _handler = new GetMotorCycleById.Handler(_repository);
    }

    [Fact]
    public async Task ShouldReturnMotorCycleWhenMotorCycleExists()
    {
        // Arrange
        var motorCycle = new MotorCycle { Id = "1", Plate = "ABC-1234" };       
        _repository.GetMotorCycleByIdAsync("1", Arg.Any<CancellationToken>()).Returns(Task.FromResult(motorCycle));        

        // Act
        var result = await _handler.Handle(new GetMotorCycleById.Query("1"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();        
    }

    [Fact]
    public async Task ShouldReturnFailureWhenMotorCycleDoesNotExists()
    {
        // Arrange
        _repository.GetMotorCycleByIdAsync("1", Arg.Any<CancellationToken>()).Returns(Task.FromResult(default(MotorCycle)));

        // Act
        var result = await _handler.Handle(new GetMotorCycleById.Query("1"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.ErrorType.Should().Be(ErrorType.NotFound);
        result.Error.Message.Should().Be("Moto não encontrada");
    }
}
