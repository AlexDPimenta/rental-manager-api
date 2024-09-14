using Carter;
using Carter.OpenApi;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Contracts.MotorCycles;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

namespace RentalManager.WebApi.Features.MotorCycles;

public class GetMotorCycleById
{
    public record Query(string Id): IRequest<Result<MotorCycleResponse>>;

    public class Handler(IMotorCycleRepository repository) : IRequestHandler<Query, Result<MotorCycleResponse>>
    {
        public async Task<Result<MotorCycleResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var motorCycle = await repository.GetMotorCycleByIdAsync(request.Id, cancellationToken);
            if (motorCycle == null)
                return Result.NotFound<MotorCycleResponse>(Error.NotFound("Moto não encontrada"));
            var response = motorCycle.Adapt<MotorCycleResponse>();
            return Result.Success(response);
        }
    }
}

public class GetMotorCycleByIdModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("motos/{id}", async ([FromRoute] string id, [FromServices] ISender sender) =>
        {
            var query = new GetMotorCycleById.Query(id);
            var result = await sender.Send(query);

            return result.IsSuccess ? Results.Ok(result.Value) : 
                result.Error.ErrorType == ErrorType.NotFound ? Results.NotFound(result.Error) : 
                Results.BadRequest(result.Error);
            
        })
            .Produces<NotFound<Error>>()
            .Produces<BadRequest<Error>>()
            .Produces<Ok<MotorCycleResponse>>()
            .WithTags("motos")
            .WithName("GetMotorCycleById")
            .WithSummary("Buscar moto baseado no id")
            .IncludeInOpenApi();
    }
}
