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

public class GetMotorCyclesByPlate
{
    public record class Query(string? Plate) : IRequest<Result<Response>>
    {       
    }

    public class Response
    {
       public IEnumerable<MotorCycleResponse> MotorCycles { get; set; }
    }

    public class Handler(IMotorCycleRepository repository) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var motorCycles = await repository.GetMotorCycleByPlateAsync(request.Plate, cancellationToken);

            if(!motorCycles.Any())
                return Result.Failure<Response>(new Error("Moto não encontrada"));

            var response = motorCycles.Adapt<IEnumerable<MotorCycleResponse>>();

            return Result.Success(new Response { MotorCycles = response });
        }
    }

    public class GetMotorCyclesByPlateModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/motos", async ([FromQuery] string? placa, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new Query(placa));

                if(result.IsFailure)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.Ok(result.Value.MotorCycles);
            })           
            .Produces<Ok<MotorCycleResponse>>()
            .WithTags("motos")
            .WithName("GetMotorCyclesByPlate")
            .WithSummary("Consultar motos existentes")
            .IncludeInOpenApi();
        }        
    }
}
