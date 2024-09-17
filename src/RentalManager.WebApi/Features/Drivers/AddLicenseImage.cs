using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.WebApi.Common;
using RentalManager.WebApi.Contracts.Drivers;
using RentalManager.WebApi.Persistence.Repository.DriversRepository;
using RentalManager.WebApi.Service;

namespace RentalManager.WebApi.Features.Drivers;

public class AddLicenseImage
{
    public record Command(string DriverId, string LicenseImage): IRequest<Result>;

    public class Handler(IDriverRepository repository, IAzureStorageService service) : IRequestHandler<Command, Result>
    {
                
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {            
            var driver = await repository.GetDriverByIdAsync(request.DriverId, cancellationToken);                       
            if (driver is null)
                return Result.Failure(Error.NotFound("Dados inválidos"));
            var fileName = $"{request.DriverId}_cnh.jpg";
            var uri = await service.UploadFileAsync(fileName, request.LicenseImage, cancellationToken);

            return Result.Success();
        }
    }    

}

public class AddLicenseImageModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/entregadores/{id}/cnh", async ([FromRoute] string id, 
            [FromBody] AddLicenseImageRequest request, [FromServices] ISender sender) =>
        {
            var command = new AddLicenseImage.Command(id, request.LicenseImage);
            var result = await sender.Send(command);

            return result.IsSuccess ? Results.Created()
                : Results.BadRequest(result.Error);
        })
        .Produces<BadRequest<Error>>()
        .Produces<Created>()
        .WithTags("entregadores")
        .WithName("AddLicenseImage")
        .WithSummary("Adicionar imagem da CNH")
        .IncludeInOpenApi();
    }
}


