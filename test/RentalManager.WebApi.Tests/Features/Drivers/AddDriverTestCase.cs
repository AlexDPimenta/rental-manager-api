using RentalManager.WebApi.Persistence.Repository.DriversRepository;
using RentalManager.WebApi.Entities;
using RentalManager.WebApi.Features.Drivers;

namespace RentalManager.WebApi.Tests.Features.Drivers;

public class AddDriverTestCase
{
    private readonly IDriverRepository repository = Substitute.For<IDriverRepository>();
    private readonly AddDriver.Handler handler;

    public AddDriverTestCase()
    {
        handler = new AddDriver.Handler(repository);
    }

    [Fact]
    public async Task ShouldAddDriver()
    {
        // Arrange
        var command = new AddDriver.Command("1", "John Doe", "123456789", new DateTime(1990, 1, 1), "123456", "A", "licenseImage");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        await repository.Received(1).AddDriverAsync(Arg.Any<Driver>(), Arg.Any<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotAddDriverWhenDriverExists()
    {
        // Arrange
        repository.GetDriverByCnpjOrLicenseNumber(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(new Driver());

        var command = new AddDriver.Command("1", "John Doe", "123456789", new DateTime(1990, 1, 1), "123456", "A", "licenseImage");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        await repository.Received(0).AddDriverAsync(Arg.Any<Driver>(), Arg.Any<CancellationToken>());

        result.IsSuccess.Should().BeFalse();
    }
}
