using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Parking.Data.Context;
using Parking.Data.Implementations;
using Parking.Data.Test.Context.Helpers;
using Parking.Domain.Entities;

namespace Parking.Data.Test.Repositories.Implementations;

public class AddressRepositoryTest
{
    private readonly Mock<ILogger<Address>> _loggerMock = new();

    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return All Addresses")]
    public async Task AddressRepository_GetAllAsync_ShouldReturnAllAddresses()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Addresses.AddRange(GetSampleAddresses());
        await context.SaveChangesAsync();

        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return Empty List When No Addresses Exist")]
    public async Task AddressRepository_GetAllAsync_ShouldReturnEmptyListWhenNoAddressesExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "GetAllAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task AddressRepository_GetAllAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllAsync());
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Address When Id Exists")]
    public async Task AddressRepository_GetByIdAsync_ShouldReturnAddressWhenIdExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Addresses.AddRange(GetSampleAddresses());
        await context.SaveChangesAsync();

        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Null When Id Does Not Exist")]
    public async Task AddressRepository_GetByIdAsync_ShouldReturnNullWhenIdDoesNotExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetByIdAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task AddressRepository_GetByIdAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetByIdAsync(1));
    }

    [Fact(DisplayName = "AddAsync - Positive: Should Return Added Address")]
    public async Task AddressRepository_AddAsync_ShouldReturnAddedAddress()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);
        var newAddress = new Address(4, "Main Street", "400", "Apt 40", "Downtown", "NY", "New York", "10001-000");

        // Act
        var result = await repository.AddAsync(newAddress);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Main Street", result.Street);
        Assert.Equal(1, context.Addresses.Count());
    }

    [Fact(DisplayName = "AddAsync - Negative: Should Throw Exception And Log Error When Insert Fails")]
    public async Task AddressRepository_AddAsync_ShouldThrowExceptionWhenInsertFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);
        var newAddress = new Address(4, "Main Street", "400", "Apt 40", "Downtown", "NY", "New York", "10001-000");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(newAddress));
    }

    [Fact(DisplayName = "UpdateAsync - Positive: Should Return Updated Address")]
    public async Task AddressRepository_UpdateAsync_ShouldReturnUpdatedAddress()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var address = new Address(1, "First Street", "100", "Apt 10", "Downtown", "NY", "New York", "10001-000");
        context.Addresses.Add(address);
        await context.SaveChangesAsync();

        context.Entry(address).State = EntityState.Detached;

        var repository = new AddressRepository(context, _loggerMock.Object);
        var updatedAddress = new Address(1, "Updated First Street", "100", "Apt 10", "Downtown", "NY", "New York", "10001-000");

        // Act
        var result = await repository.UpdateAsync(updatedAddress);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated First Street", result.Street);
    }

    [Fact(DisplayName = "UpdateAsync - Negative: Should Throw Exception And Log Error When Update Fails")]
    public async Task AddressRepository_UpdateAsync_ShouldThrowExceptionWhenUpdateFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);
        var updatedAddress = new Address(1, "Updated First Street", "100", "Apt 10", "Downtown", "NY", "New York", "10001-000");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(updatedAddress));
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Complete Successfully When Address Exists")]
    public async Task AddressRepository_DeleteAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var address = new Address(1, "First Street", "100", "Apt 10", "Downtown", "NY", "New York", "10001-000");
        context.Addresses.Add(address);
        await context.SaveChangesAsync();

        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        Assert.Equal(0, context.Addresses.Count());
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Handle Non Existing Address Gracefully")]
    public async Task AddressRepository_DeleteAsync_ShouldHandleNonExistingAddressGracefully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(99));

        // Assert
        Assert.Null(exception);
    }

    [Fact(DisplayName = "DeleteAsync - Negative: Should Throw Exception And Log Error When Delete Fails")]
    public async Task AddressRepository_DeleteAsync_ShouldThrowExceptionWhenDeleteFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new AddressRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DeleteAsync(1));
    }

    private List<Address> GetSampleAddresses() => new()
    {
        new Address(1, "First Street", "100", "Apt 10", "Downtown", "NY", "New York", "10001-000"),
        new Address(2, "Second Street", "200", "House", "Uptown", "CA", "Los Angeles", "90001-000"),
        new Address(3, "Third Street", "300", null, "Midtown", "FL", "Miami", "33101-000")
    };
}