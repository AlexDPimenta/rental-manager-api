using MassTransit;
using RentalManager.WebApi.Contracts.MotorCycles;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Events;
using RentalManager.WebApi.Features.MotorCycles;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

namespace RentalManager.WebApi.Tests.Features.MotorCycles;

public sealed class AddMotorCycleTestCase
{
    private readonly IMotorCycleRepository _repository = Substitute.For<IMotorCycleRepository>();
    private readonly ITopicProducer<MotorCycleCreated> _producer = Substitute.For<ITopicProducer<MotorCycleCreated>>();
    private readonly AddMotorCycle.Handler _handler;

    public AddMotorCycleTestCase()
    {
        _handler = new AddMotorCycle.Handler(_repository, _producer);
    }

    [Fact]
    public async Task Handle_WhenMotorCycleExists_ReturnsFailure()
    {
        // Arrange
        var request = new AddMotorCycle.Command("test-moto", 2020, "test-model", "test-plate");
        _repository.GetMotorCycleByPlateAsync(request.Plate, Arg.Any<CancellationToken>()).Returns(new List<MotorCycle>() { new MotorCycle { Plate = request.Plate } });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Contain("Moto já cadastrada");        
    }

    [Fact]
    public async Task Handle_WhenYearIs2024_ProducesMotorCycleCreatedEvent()
    {
        // Arrange
        var request = new AddMotorCycle.Command("test-moto", 2024, "test-model", "test-plate");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _producer.Received().Produce(Arg.Is<MotorCycleCreated>(m => m.Plate == request.Plate));
        result.IsSuccess.Should().BeTrue();
    }



}
