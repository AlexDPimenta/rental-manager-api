using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Features.MotorCycles;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;
using System.Runtime.CompilerServices;

namespace RentalManager.WebApi.Tests.Features.MotorCycles;

public class RemoveMotorCycleByIdTestCase
{
    private readonly IMotorCycleRepository motorCycleRepository = Substitute.For<IMotorCycleRepository>();
    private readonly RemoveMotorCycleById.Handler handler;
    public RemoveMotorCycleByIdTestCase()
    {
        handler = new RemoveMotorCycleById.Handler(motorCycleRepository);
    }

    [Fact]
    public async Task ShouldRemoveMotorCycleById()
    {
        // Arrange
        var motorCycle = new MotorCycle
        {
            Id = "1",
            Year = 2021,
            Model = "Model",
            Plate = "Plate"
        };

        motorCycleRepository.GetMotorCycleByIdAsync("1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(motorCycle));

        // Act
        var response = await handler.Handle(new RemoveMotorCycleById.Command("1"), CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();        
    }

    [Fact]
    public async Task ShouldNotRemoveMotorCycleByIdWhenMotorCycleIsNull()
    {
        // Arrange
        motorCycleRepository.GetMotorCycleByIdAsync("1", Arg.Any<CancellationToken>())
            .Returns((MotorCycle)null);

        // Act
        var response = await handler.Handle(new RemoveMotorCycleById.Command("1"), CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Error.Message.Should().Be("Dados inválidos");
    }
}
