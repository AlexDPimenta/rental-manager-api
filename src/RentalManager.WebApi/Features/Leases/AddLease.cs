using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Persistence.Context;
using RentalManager.WebApi.Persistence.Repository.LeaseRepository;
using Mapster;
using RentalManager.WebApi.Entities;
using Carter;
using RentalManager.WebApi.Contracts.Leases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Carter.OpenApi;

namespace RentalManager.WebApi.Features.Leases;

public class AddLease
{
    public record Command(
        string DriverId,
        string MotorCycleId,
        int DurationInDays,
        DateTime StartDate,
        DateTime EndDate,
        DateTime ExpectedEndDate
    ) : IRequest<Result>;

    public class Handler(ILeaseRepository repository, RentalManagerDbContext context) : IRequestHandler<Command, Result>
    {       

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var driver = await context.Drivers                
                .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

            var motorCycle = await context.MotorCycles
                .FirstOrDefaultAsync(m => m.Id == request.MotorCycleId);

            var plan = await context.Plans
               .FirstOrDefaultAsync(m => m.DurationInDays == request.DurationInDays);

            if (driver == null || motorCycle == null || plan == null)
            {
                return Result.Failure(Error.Failure("Dados inválidos!"));
            }

            if (!driver.LicenseCategory.ToLower().Contains("a"))
                return Result.Failure(Error.Failure("Dados inválidos"));
            
            if(request.StartDate > request.EndDate || request.StartDate > request.ExpectedEndDate)
                return Result.Failure(Error.Failure("Dados inválidos"));

            var lease = request.Adapt<Lease>();

            await repository.AddLeaseAsync(lease, cancellationToken);

            return Result.Success();
        }
    }
}

public class AddLeaseModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/locacao", async ([FromServices] ISender sender, [FromBody] LeaseRequest request) =>
        {
            var command = request.Adapt<AddLease.Command>();

            var result = await sender.Send(command);

            return result.IsSuccess ? Results.Created() :
                Results.BadRequest(result.Error);
        })
         .Produces<BadRequest<Error>>()
         .Produces<Created>()
         .WithTags("locação")
         .WithName("AddLease")
         .WithSummary("Cadastrar um novo aluguel")
         .IncludeInOpenApi(); ;
    }
}
