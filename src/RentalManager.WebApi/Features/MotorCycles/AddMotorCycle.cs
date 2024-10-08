﻿using Carter;
using Carter.OpenApi;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Contracts.MotorCycles;
using RentalManager.WebApi.Events;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;

namespace RentalManager.WebApi.Features.MotorCycles;

public class AddMotorCycle
{  
    public record Command(string Id,
        int Year,
        string Model,
        string Plate): IRequest<Result<MotorCycleResponse>>;
    public class Handler(IMotorCycleRepository repository, ITopicProducer<MotorCycleCreated> producer) : IRequestHandler<Command, Result<MotorCycleResponse>>
    {
        public async Task<Result<MotorCycleResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var motorCycles = await repository.GetMotorCycleByPlateAsync(request.Plate, cancellationToken);
            if (motorCycles.Any())
                return Result.Failure<MotorCycleResponse>(Error.Failure("Moto já cadastrada"));
            
            if (request.Year == 2024)
            {
                var motoCreated = request.Adapt<MotorCycleCreated>();
                await producer.Produce(motoCreated);
            }
                            
            return Result.Success(new MotorCycleResponse { Id = request.Id });

        }
    }

    public class AddMotorCycleModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/motos", async ([FromBody] MotorCycleRequest request, [FromServices] ISender sender) =>
            {
                var command = request.Adapt<Command>();
                var result = await sender.Send(command);

                return result.IsSuccess ? Results.Created()
                : Results.BadRequest(result.Error);
            })
            .Produces<BadRequest<Error>>()
            .Produces<Created<MotorCycleResponse>>()
            .WithTags("motos")
            .WithName("AddMotorCycle")
            .WithSummary("Cadastrar uma nova moto")
            .IncludeInOpenApi();
        }
    }
}
