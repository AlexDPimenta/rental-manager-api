using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Contracts.Leases;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Persistence.Repository.LeaseRepository;
using System.Text.Json.Serialization;
using static RentalManager.WebApi.Features.Leases.UpdateLeaseById;

namespace RentalManager.WebApi.Features.Leases;

public class UpdateLeaseById
{
    public record UpdateLeaseByIdResponse()
    {
        [JsonPropertyName("mensagem")]
        public string Message { get; init; } = "Data de devolução informada com sucesso";
        [JsonPropertyName("valor_total")]
        public double TotalCost { get; set; }
    }

    public record Command(string id, DateTime returnData): IRequest<Result<UpdateLeaseByIdResponse>>;

    public class Handler(ILeaseRepository repository) : IRequestHandler<Command, Result<UpdateLeaseByIdResponse>>
    {
        public async Task<Result<UpdateLeaseByIdResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var lease = await repository.GetLeaseByIdAsync(request.id, cancellationToken);

            if (lease == null)
                return Result.Failure<UpdateLeaseByIdResponse>(Error.Failure("Dados inválidos"));
            var response = new UpdateLeaseByIdResponse { TotalCost = (double)lease.LeasePlan.CostPerDay * lease.DurationInDays };
            if (request.returnData.Date < lease.ExpectedEndDate.Date)
            {
                var totalCost = CalculateEarlyReturnCost(lease.DurationInDays, lease.StartDate.AddDays(1), 
                    request.returnData, lease.ExpectedEndDate, lease.LeasePlan.CostPerDay);
                response.TotalCost = (double)totalCost;
            }

            if(request.returnData.Date > lease.ExpectedEndDate.Date)
            {
                var totalCost = CalculateLateReturnCost(lease.DurationInDays, request.returnData, 
                    expectedEndDate: lease.ExpectedEndDate, lease.LeasePlan.CostPerDay);
                response.TotalCost = (double)totalCost;
            }

            lease.ReturnData = request.returnData;
            await repository.UpdateLeaseAsync(lease, cancellationToken);

            return Result.Success(response);
        }

        private decimal CalculateEarlyReturnCost(int daysInPeriod, 
            DateTime startDate, DateTime returnData, DateTime expectedEndDate, decimal costPerDay)
        {
            var daysNotUsed = (expectedEndDate.Date - returnData.Date).Days;            
            var penaltyRate = daysInPeriod == 7 ? 0.20m : daysInPeriod == 15 ? 0.40m : 0m;            
            var penalty = daysNotUsed * costPerDay * penaltyRate;            
            var daysUsed = (returnData.Date - startDate.Date).Days;
            var costOfUsedDays = daysUsed * costPerDay;
            
            return costOfUsedDays + penalty;
           
        }
        
        private decimal CalculateLateReturnCost(int daysInPeriod, DateTime returnData, DateTime expectedEndDate, decimal costPerDay)
        {            
            var daysLate = (returnData.Date - expectedEndDate.Date).Days;
            var lateCost = daysLate * costPerDay;
            var latePenalty = daysLate * 50m;
            var normalCost = daysInPeriod * costPerDay;
            var penaltyCost = lateCost + latePenalty;
            return penaltyCost + normalCost;
        }
    }    
}

public class UpdateLeaseByIdModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/locacao/{id}", async ([FromRoute] string id, [FromBody] UpdateLeaseRequest request, [FromServices] ISender sender) =>
        {
            var command = new UpdateLeaseById.Command(id, request.ReturnData);
            var result = await sender.Send(command);

            

            return result.IsSuccess ? Results.Ok(result.Value) :
                Results.BadRequest(result.Error);
        })
            .Produces<BadRequest<Error>>()
            .Produces<Ok<UpdateLeaseByIdResponse>>()
            .WithTags("locação")
            .WithName("UpdateLeaseById")
            .WithSummary("Atualizar data de devolução do aluguel")
            .IncludeInOpenApi();
    }
}
