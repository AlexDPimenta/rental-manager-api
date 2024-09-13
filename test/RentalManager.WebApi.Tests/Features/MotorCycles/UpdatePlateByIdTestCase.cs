using RentalManager.WebApi.Features.MotorCycles;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Persistence;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;
using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Tests.Features.MotorCycles;

public class UpdatePlateByIdTestCase
{

    private readonly UpdatePlateById.Handler _handler;
    private readonly IMotorCycleRepository _repository = Substitute.For<IMotorCycleRepository>();

    public UpdatePlateByIdTestCase()
    {
        _handler = new UpdatePlateById.Handler(_repository);
    }

    [Fact]
    public async Task ShouldReturnSuccessWhenUpdatePlateById()
    {
        // Arrange
        var motorCycle = new MotorCycle { Id = "1", Plate = "ABC-1234" };
        _repository.GetMotorCycleByIdAsync("1", Arg.Any<CancellationToken>()).Returns(motorCycle);

        // Act
        var result = await _handler.Handle(new UpdatePlateById.Command("1", "DEF-5678"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Message.Should().Be("Placa modificada com sucesso");        
    }

}
