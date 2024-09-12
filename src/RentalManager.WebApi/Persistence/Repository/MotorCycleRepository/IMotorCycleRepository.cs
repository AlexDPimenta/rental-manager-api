using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

public interface IMotorCycleRepository
{
    Task<MotorCycle> AddMotorCycleAsync(MotorCycle motorCycle, CancellationToken cancellationToken);
    Task<IEnumerable<MotorCycle>> GetMotorCycleByPlateAsync(string plate, CancellationToken cancellationToken);
}
