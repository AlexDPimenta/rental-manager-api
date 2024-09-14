using RentalManager.WebApi.Entities;

namespace RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

public interface IMotorCycleRepository
{
    Task<MotorCycle> AddMotorCycleAsync(MotorCycle motorCycle, CancellationToken cancellationToken);
    Task<IEnumerable<MotorCycle>> GetMotorCycleByPlateAsync(string plate, CancellationToken cancellationToken);
    Task<MotorCycle> GetMotorCycleByIdAsync(string id, CancellationToken cancellationToken);
    Task UpdatePlateByIdAsync(MotorCycle motorCycle, CancellationToken cancellationToken);
    Task RemoveMotorCycleAsync(MotorCycle motorCycle, CancellationToken cancellationToken);
}
