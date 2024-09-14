using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

namespace RentalManager.WebApi.Features.MotorCycles;

public class RemoveMotorCycleById
{
    public record Command(string Id) : IRequest<Result>;  

    public class Handler(IMotorCycleRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var motorCycle = await repository.GetMotorCycleByIdAsync(request.Id, cancellationToken);
            if (motorCycle is null || motorCycle.Leases.Any())
                return Result.Failure(Error.Failure("Dados inválidos")); 

            await repository.RemoveMotorCycleAsync(motorCycle, cancellationToken);

            return Result.Success();


        }
    }
}

public class RemoveMotorCycleByIdModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/motos/{id}", async ([FromRoute] string id, [FromServices] ISender sender) =>
        {
            var command = new RemoveMotorCycleById.Command(id);
            var result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok()
            : Results.BadRequest(result.Error);
        })
            .Produces<BadRequest<Error>>()
            .Produces<Ok>()
            .WithTags("motos")
            .WithName("RemoveMotorCycleById")
            .WithSummary("Remover moto baseado no id")
            .IncludeInOpenApi();
    }
}
