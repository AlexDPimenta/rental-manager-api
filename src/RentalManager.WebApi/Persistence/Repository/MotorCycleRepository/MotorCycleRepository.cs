using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Persistence.Context;

namespace RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

public class MotorCycleRepository(RentalManagerDbContext dbContext) : IMotorCycleRepository
{

    public async Task<MotorCycle> AddMotorCycleAsync(MotorCycle motorCycle, 
        CancellationToken cancellationToken)
    {
        await dbContext.MotorCycles.AddAsync(motorCycle, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return motorCycle;
    }

    public async Task<IEnumerable<MotorCycle>> GetMotorCycleByPlateAsync(string plate, CancellationToken cancellationToken)
    {        
        return await dbContext.MotorCycles
            .Where(m => string.IsNullOrEmpty(plate) || m.Plate == plate)
            .ToListAsync(cancellationToken);

    }
}
