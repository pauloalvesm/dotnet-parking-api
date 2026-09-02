using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Parking.Data.Context;
using Parking.Data.Implementations;
using Parking.Data.Test.Context.Helpers;
using Parking.Domain.Entities;
using Parking.Domain.Enums;

namespace Parking.Data.Test.Repositories.Implementations;

public class CustomerVehicleRepositoryTest
{
    private readonly Mock<ILogger<CustomerVehicle>> _loggerMock = new();

    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return All CustomerVehicles With Navigation Properties")]
    public async Task CustomerVehicleRepository_GetAllAsync_ShouldReturnAllCustomerVehiclesWithNavigationProperties()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var customer = new Customer(1, "John Doe", new DateOnly(1990, 1, 15), "11122233344", "5511911112222", "john.doe@email.com", 1);
        var vehicle = new Vehicle(1, VehicleType.Car, "Brand", "Sedan", "Blue", 2020, "No notes");
        var customerVehicle = new CustomerVehicle(1, 1, 1);

        context.Customers.Add(customer);
        context.Vehicles.Add(vehicle);
        context.CustomerVehicles.Add(customerVehicle);
        await context.SaveChangesAsync();

        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var item = result.First();
        Assert.NotNull(item.Customer);
        Assert.NotNull(item.Vehicle);
        Assert.Equal("John Doe", item.Customer.Name);
        Assert.Equal("Sedan", item.Vehicle.Model);
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return Empty List When No CustomerVehicles Exist")]
    public async Task CustomerVehicleRepository_GetAllAsync_ShouldReturnEmptyListWhenNoCustomerVehiclesExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "GetAllAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task CustomerVehicleRepository_GetAllAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=invalid_host;Database=invalid_db;Username=invalid;Password=invalid;Timeout=1")
            .Options;

        using var context = new ApplicationDbContext(options);
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllAsync());
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return CustomerVehicle When Id Exists")]
    public async Task CustomerVehicleRepository_GetByIdAsync_ShouldReturnCustomerVehicleWhenIdExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var customerVehicle = new CustomerVehicle(1, 1, 1);
        context.CustomerVehicles.Add(customerVehicle);
        await context.SaveChangesAsync();

        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Null When Id Does Not Exist")]
    public async Task CustomerVehicleRepository_GetByIdAsync_ShouldReturnNullWhenIdDoesNotExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetByIdAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task CustomerVehicleRepository_GetByIdAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetByIdAsync(1));
    }

    [Fact(DisplayName = "AddAsync - Positive: Should Return Added CustomerVehicle")]
    public async Task CustomerVehicleRepository_AddAsync_ShouldReturnAddedCustomerVehicle()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);
        var newCustomerVehicle = new CustomerVehicle(1, 1, 1);

        // Act
        var result = await repository.AddAsync(newCustomerVehicle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, context.CustomerVehicles.Count());
    }

    [Fact(DisplayName = "AddAsync - Negative: Should Throw Exception And Log Error When Insert Fails")]
    public async Task CustomerVehicleRepository_AddAsync_ShouldThrowExceptionWhenInsertFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);
        var newCustomerVehicle = new CustomerVehicle(1, 1, 1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(newCustomerVehicle));
    }

    [Fact(DisplayName = "UpdateAsync - Positive: Should Return Updated CustomerVehicle")]
    public async Task CustomerVehicleRepository_UpdateAsync_ShouldReturnUpdatedCustomerVehicle()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var customerVehicle = new CustomerVehicle(1, 1, 1);
        context.CustomerVehicles.Add(customerVehicle);
        await context.SaveChangesAsync();

        context.Entry(customerVehicle).State = EntityState.Detached;

        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);
        var updatedCustomerVehicle = new CustomerVehicle(1, 2, 2);

        // Act
        var result = await repository.UpdateAsync(updatedCustomerVehicle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.CustomerId);
        Assert.Equal(2, result.VehicleId);
    }

    [Fact(DisplayName = "UpdateAsync - Negative: Should Throw Exception And Log Error When Update Fails")]
    public async Task CustomerVehicleRepository_UpdateAsync_ShouldThrowExceptionWhenUpdateFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);
        var updatedCustomerVehicle = new CustomerVehicle(1, 1, 1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(updatedCustomerVehicle));
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Complete Successfully When CustomerVehicle Exists")]
    public async Task CustomerVehicleRepository_DeleteAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var customerVehicle = new CustomerVehicle(1, 1, 1);
        context.CustomerVehicles.Add(customerVehicle);
        await context.SaveChangesAsync();

        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        Assert.Equal(0, context.CustomerVehicles.Count());
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Handle Non Existing CustomerVehicle Gracefully")]
    public async Task CustomerVehicleRepository_DeleteAsync_ShouldHandleNonExistingCustomerVehicleGracefully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(99));

        // Assert
        Assert.Null(exception);
    }

    [Fact(DisplayName = "DeleteAsync - Negative: Should Throw Exception And Log Error When Delete Fails")]
    public async Task CustomerVehicleRepository_DeleteAsync_ShouldThrowExceptionWhenDeleteFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerVehicleRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DeleteAsync(1));
    }
}