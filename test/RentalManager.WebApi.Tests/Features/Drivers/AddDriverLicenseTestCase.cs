using RentalManager.WebApi.Contracts.Drivers;
using RentalManager.WebApi.Persistence.Repository.DriversRepository;
using RentalManager.WebApi.Features.Drivers;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Service;

namespace RentalManager.WebApi.Tests.Features.Drivers;

public class AddDriverLicenseTestCase
{
    private readonly IDriverRepository driverRepository = Substitute.For<IDriverRepository>();
    private readonly IAzureStorageService azureStorageService = Substitute.For<IAzureStorageService>(); 
    private readonly AddLicenseImage.Handler _handler;

    public AddDriverLicenseTestCase()
    {
        _handler = new AddLicenseImage.Handler(driverRepository, azureStorageService);
    }

    [Fact]
    public async Task ShouldAddDriverLicense()
    {
        // Arrange
        var command = new AddLicenseImage.Command("driverId", "base64Image");        

        var driver = new Driver
        {
            Id = "driverId",
            LicenseImage = "licenseImage"
        };

        driverRepository.GetDriverByIdAsync("driverId", Arg.Any<CancellationToken>()).Returns(driver);

        azureStorageService.UploadFileAsync("driverId", "base64Image", Arg.Any<CancellationToken>()).Returns("url");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await driverRepository.Received(1).GetDriverByIdAsync("driverId", Arg.Any<CancellationToken>());
        await azureStorageService.Received(1).UploadFileAsync("driverId_cnh.jpg", "base64Image", Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();        
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDriverIsNull()
    {
        // Arrange
        var command = new AddLicenseImage.Command("driverId", "base64Image");

        driverRepository.GetDriverByIdAsync("driverId", Arg.Any<CancellationToken>()).Returns((Driver)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await driverRepository.Received(1).GetDriverByIdAsync("driverId", Arg.Any<CancellationToken>());
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Contains("Dados inválidos");
    }
}
