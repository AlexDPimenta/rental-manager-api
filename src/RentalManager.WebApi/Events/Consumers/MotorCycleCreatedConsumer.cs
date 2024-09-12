using MassTransit;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;
using Mapster;
using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Events.Consumers;

public class MotorCycleCreatedConsumer(IMotorCycleRepository repository) : IConsumer<MotorCycleCreated>
{
    public async Task Consume(ConsumeContext<MotorCycleCreated> context)
    {
        var motorCycle = context.Message.Adapt<MotorCycle>();
        await repository.AddMotorCycleAsync(motorCycle, context.CancellationToken);
    }
}
