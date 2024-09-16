using MediatR;
using RentalManager.WebApi.Common;
using Mapster;
using RentalManager.WebApi.Persistence.Repository.DriversRepository;
using RentalManager.WebApi.Entities;
using Carter;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Contracts.Drivers;
using RentalManager.WebApi.Features.Drivers;
using Carter.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using RentalManager.WebApi.Contracts.MotorCycles;

namespace RentalManager.WebApi.Features.Drivers
{
    public class AddDriver
    {
        public record Command(string Id, string Name, string Cnpj, DateTime BirthdayDate,
            string LicenseNumber, string LicenseCategory, string LicenseImage) : IRequest<Result>;

        public class Handler(IDriverRepository repository) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var driverExists = await repository.GetDriverByCnpjOrLicenseNumber(request.Cnpj, request.LicenseNumber, cancellationToken) != null;
                if (driverExists || request.LicenseCategory.ToLower() is not ("a" or "b" or "ab") )
                {
                    return Result.Failure(Error.Failure("Dados inválidos"));
                }

                var driver = request.Adapt<Driver>();

                await repository.AddDriverAsync(driver, cancellationToken);

                return Result.Success();
            }
        }
    }
}

public class AddDriverModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/entregadores", async ([FromServices] ISender sender, [FromBody] AddDriverRequest request) =>
        {
            var command = new AddDriver.Command(request.Id, request.Name, request.Cnpj, request.BirthdayDate, request.LicenseNumber,
                request.LicenseCategory, request.LicenseImage);

            var result = await sender.Send(command);

            return result.IsSuccess ? Results.Created() :
                Results.BadRequest(result.Error);
        })
         .Produces<BadRequest<Error>>()
         .Produces<Created<MotorCycleResponse>>()
         .WithTags("entregadores")
         .WithName("AddDriver")
         .WithSummary("Cadastrar um novo entregador")
         .IncludeInOpenApi(); ;
    }
}
