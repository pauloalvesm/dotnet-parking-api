using Microsoft.Extensions.Configuration;
using Moq;
using Parking.Data.Context;
using Parking.Data.Factories.Implementations;

namespace Parking.Data.Test.Context.Factories;

public class ApplicationDbContextFactoryTest
{
    private readonly Mock<IServiceProvider> _serviceProviderMock = new();

    [Fact(DisplayName = "CreateDbContext - Should Create ApplicationDbContext When ConnectionString Is Valid")]
    public void CreateDbContext_ShouldCreateApplicationDbContextWhenConnectionStringIsValid()
    {
        // Arrange
        var connectionString = "Server=localhost;Database=Parking;User Id=postgres;Password=postgres;";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", connectionString }
            })
            .Build();

        var factory = new ApplicationDbContextFactory(_serviceProviderMock.Object, configuration);

        // Act
        var context = factory.CreateDbContext();

        // Assert
        Assert.NotNull(context);
        Assert.IsType<ApplicationDbContext>(context);
    }

    [Theory(DisplayName = "CreateDbContext - Should Throw InvalidOperationException When ConnectionString Is Missing Or Empty")]
    [InlineData(null)]
    [InlineData("")]
    public void CreateDbContext_ShouldThrowExceptionWhenConnectionStringIsMissingOrEmpty(string? connectionString)
    {
        // Arrange
        var initialData = new Dictionary<string, string?>();
        if (connectionString != null)
        {
            initialData["ConnectionStrings:DefaultConnection"] = connectionString;
        }

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(initialData)
            .Build();

        var factory = new ApplicationDbContextFactory(_serviceProviderMock.Object, configuration);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateDbContext());
        Assert.Equal("Connection string 'DefaultConnection' not found.", exception.Message);
    }
}