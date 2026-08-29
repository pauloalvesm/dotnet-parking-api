using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Parking.Data.Context;
using Parking.Data.Implementations;
using Parking.Data.Test.Context.Helpers;
using Parking.Domain.Entities;
using Parking.Domain.Enums;

namespace Parking.Data.Test.Repositories.Implementations;

public class RepositoryTest
{
    private readonly Mock<ILogger<Vehicle>> _loggerMock = new();

    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return All Entities")]
    public async Task Repository_GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Vehicles.AddRange(GetSampleVehicles());
        await context.SaveChangesAsync();

        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return Empty List When No Entities Exist")]
    public async Task Repository_GetAllAsync_ShouldReturnEmptyListWhenNoEntitiesExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "GetAllAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task Repository_GetAllAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllAsync());
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Entity When Id Exists")]
    public async Task Repository_GetByIdAsync_ShouldReturnEntityWhenIdExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Vehicles.AddRange(GetSampleVehicles());
        await context.SaveChangesAsync();

        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Null When Id Does Not Exist")]
    public async Task Repository_GetByIdAsync_ShouldReturnNullWhenIdDoesNotExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetByIdAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task Repository_GetByIdAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetByIdAsync(1));
    }

    [Fact(DisplayName = "AddAsync - Positive: Should Return Added Entity")]
    public async Task Repository_AddAsync_ShouldReturnAddedEntity()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);
        var newVehicle = new Vehicle(4, VehicleType.Motorcycle, "Honda", "CB 500", "Black", 2021, "New");

        // Act
        var result = await repository.AddAsync(newVehicle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Honda", result.Brand);
        Assert.Equal(1, context.Vehicles.Count());
    }

    [Fact(DisplayName = "AddAsync - Negative: Should Throw Exception And Log Error When Insert Fails")]
    public async Task Repository_AddAsync_ShouldThrowExceptionWhenInsertFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);
        var newVehicle = new Vehicle(4, VehicleType.Motorcycle, "Honda", "CB 500", "Black", 2021, "New");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(newVehicle));
    }

    [Fact(DisplayName = "UpdateAsync - Positive: Should Return Updated Entity")]
    public async Task Repository_UpdateAsync_ShouldReturnUpdatedEntity()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var vehicle = new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Blue", 2018, "Dented side");
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        context.Entry(vehicle).State = EntityState.Detached;

        var repository = new Repository<Vehicle>(context, _loggerMock.Object);
        var updatedVehicle = new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Green", 2019, "Refurbished");

        // Act
        var result = await repository.UpdateAsync(updatedVehicle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Green", result.Color);
    }

    [Fact(DisplayName = "UpdateAsync - Negative: Should Throw Exception And Log Error When Update Fails")]
    public async Task Repository_UpdateAsync_ShouldThrowExceptionWhenUpdateFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);
        var updatedVehicle = new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Green", 2019, "Refurbished");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(updatedVehicle));
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Complete Successfully When Entity Exists")]
    public async Task Repository_DeleteAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var vehicle = new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Blue", 2018, "Dented side");
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        Assert.Equal(0, context.Vehicles.Count());
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Handle Non Existing Entity Gracefully")]
    public async Task Repository_DeleteAsync_ShouldHandleNonExistingEntityGracefully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(99));

        // Assert
        Assert.Null(exception);
    }

    [Fact(DisplayName = "DeleteAsync - Negative: Should Throw Exception And Log Error When Delete Fails")]
    public async Task Repository_DeleteAsync_ShouldThrowExceptionWhenDeleteFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new Repository<Vehicle>(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DeleteAsync(1));
    }

    private List<Vehicle> GetSampleVehicles() => new()
    {
        new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Blue", 2018, "Dented side"),
        new Vehicle(2, VehicleType.Car, "Volkswagen", "Gol", "White", 2016, "Cracked headlight"),
        new Vehicle(3, VehicleType.Motorcycle, "Yamaha", "FZ25", "Blue", 2022, "Scratches on gas tank")
    };
}