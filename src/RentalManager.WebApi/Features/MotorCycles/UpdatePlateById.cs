using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Contracts.MotorCycles;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

namespace RentalManager.WebApi.Features.MotorCycles;

public class UpdatePlateById
{
    public record Command(string Id, string Plate) : IRequest<Result<UpdatePlateByIdResponse>>;

    public class Handler(IMotorCycleRepository repository) : IRequestHandler<Command, Result<UpdatePlateByIdResponse>>
    {
        public async Task<Result<UpdatePlateByIdResponse>> Handle(Command command, CancellationToken cancellationToken)
        {
            var motorCycle = await repository.GetMotorCycleByIdAsync(command.Id, cancellationToken);
            if (motorCycle == null)
                return Result.Failure<UpdatePlateByIdResponse>(Error.Failure("Dados inválidos"));

            motorCycle.Plate = command.Plate;

            await repository.UpdatePlateByIdAsync(motorCycle, cancellationToken);

            return Result.Success(new UpdatePlateByIdResponse { Message = "Placa modificada com sucesso" });

        }
    }

}

public class UpdateByPlateIdModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/motos/{id}/placa", async ([FromRoute] string id, [FromBody] UpdatePlateByIdRequest request, [FromServices] ISender sender) =>
        {
            var command = new UpdatePlateById.Command(id, request.Plate);
            var result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
        })
            .Produces<BadRequest<Error>>()
            .Produces<Ok<MotorCycleResponse>>()
            .WithTags("motos")
            .WithName("UpdatePlateById")
            .WithSummary("Atualizar placa da moto baseado no id")
            .IncludeInOpenApi(); 
    }
}
