using Carter;
using Carter.OpenApi;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Contracts.Leases;
using RentalManager.WebApi.Features.Leases;
using RentalManager.WebApi.Persistence.Repository.LeaseRepository;

namespace RentalManager.WebApi.Features.Leases
{
    public class GetLeaseById
    {
        public record Query(string Id) : IRequest<Result<LeaseResponse>>;        

        public class Handler(ILeaseRepository repository) : IRequestHandler<Query, Result<LeaseResponse>>
        {
            public async Task<Result<LeaseResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lease = await repository.GetLeaseByIdAsync(request.Id, cancellationToken);
                if (lease == null)                
                    return Result.Failure<LeaseResponse>(Error.Failure("Dados inválidos"));                

                var response = lease.Adapt<LeaseResponse>();
                response.CostPerDay = lease.LeasePlan.CostPerDay;
                return Result.Success(response);
            }
        }
    }
}

public class GetLeaseByIdModule: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/locacao/{id}", async ([FromRoute] string id, [FromServices] ISender sender) =>
        {
            var query = new GetLeaseById.Query(id);
            var result = await sender.Send(query);

            return result.IsSuccess ? Results.Ok(result.Value) :
                Results.BadRequest(result.Error);
        })
            .Produces<BadRequest<Error>>()
            .Produces<Ok<LeaseResponse>>()
            .WithTags("locação")
            .WithName("GetLeaseById")
            .WithSummary("Obter aluguel baseado no id")
            .IncludeInOpenApi();
    }
}
