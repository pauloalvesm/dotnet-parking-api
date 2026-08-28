using Moq;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.Interfaces.Repositories;

namespace Parking.Data.Test.Repositories.Implementations;

public class RepositoryTest
{
    private readonly List<Vehicle> _listVehicles = new()
    {
        new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Blue", 2018, "Dented side"),
        new Vehicle(2, VehicleType.Car, "Volkswagen", "Gol", "White", 2016, "Cracked headlight"),
        new Vehicle(3, VehicleType.Motorcycle, "Yamaha", "FZ25", "Blue", 2022, "Scratches on gas tank")
    };

    [Fact(DisplayName = "GetAllAsync - Should Return All Entities")]
    public async Task Repository_GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_listVehicles);

        // Act
        var result = await repositoryMock.Object.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_listVehicles.Count, result.Count());
        Assert.Equal(_listVehicles, result);
    }

    [Fact(DisplayName = "GetAllAsync - Should Return Empty List When No Entities Exist")]
    public async Task Repository_GetAllAsync_ShouldReturnEmptyListWhenNoEntitiesExist()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Vehicle>());

        // Act
        var result = await repositoryMock.Object.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "GetAllAsync - Should Throw Exception When Query Fails")]
    public async Task Repository_GetAllAsync_ShouldThrowExceptionWhenQueryFails()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        var expectedErrorMessage = "Error when searching list of records";
        repositoryMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception(expectedErrorMessage));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => repositoryMock.Object.GetAllAsync());

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

    [Fact(DisplayName = "GetByIdAsync - Should Return Entity When Id Exists")]
    public async Task Repository_GetByIdAsync_ShouldReturnEntityWhenIdExists()
    {
        // Arrange
        int entityId = 1;
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        repositoryMock.Setup(r => r.GetByIdAsync(entityId))
                      .ReturnsAsync(_listVehicles.FirstOrDefault(v => v.Id == entityId));

        // Act
        var result = await repositoryMock.Object.GetByIdAsync(entityId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityId, result.Id);
    }

    [Fact(DisplayName = "GetByIdAsync - Should Return Null When Id Does Not Exist")]
    public async Task Repository_GetByIdAsync_ShouldReturnNullWhenIdDoesNotExist()
    {
        // Arrange
        int entityId = 99;
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        repositoryMock.Setup(r => r.GetByIdAsync(entityId)).ReturnsAsync((Vehicle)null);

        // Act
        var result = await repositoryMock.Object.GetByIdAsync(entityId);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetByIdAsync - Should Throw Exception When Query Fails")]
    public async Task Repository_GetByIdAsync_ShouldThrowExceptionWhenQueryFails()
    {
        // Arrange
        int entityId = 1;
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        var expectedErrorMessage = "Error getting record with ID";
        repositoryMock.Setup(r => r.GetByIdAsync(entityId)).ThrowsAsync(new Exception(expectedErrorMessage));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => repositoryMock.Object.GetByIdAsync(entityId));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

    [Fact(DisplayName = "AddAsync - Should Return Added Entity")]
    public async Task Repository_AddAsync_ShouldReturnAddedEntity()
    {
        // Arrange
        var newVehicle = new Vehicle(4, VehicleType.Motorcycle, "Honda", "CB 500", "Black", 2021, "New");
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        repositoryMock.Setup(r => r.AddAsync(It.IsAny<Vehicle>())).ReturnsAsync(newVehicle);

        // Act
        var result = await repositoryMock.Object.AddAsync(newVehicle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Honda", result.Brand);
        Assert.Equal(4, result.Id);
    }

    [Fact(DisplayName = "AddAsync - Should Throw Exception When Insert Fails")]
    public async Task Repository_AddAsync_ShouldThrowExceptionWhenInsertFails()
    {
        // Arrange
        var newVehicle = new Vehicle(4, VehicleType.Motorcycle, "Honda", "CB 500", "Black", 2021, "New");
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        var expectedErrorMessage = "Error when adding a new record";
        repositoryMock.Setup(r => r.AddAsync(It.IsAny<Vehicle>())).ThrowsAsync(new Exception(expectedErrorMessage));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => repositoryMock.Object.AddAsync(newVehicle));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

    [Fact(DisplayName = "UpdateAsync - Should Return Updated Entity")]
    public async Task Repository_UpdateAsync_ShouldReturnUpdatedEntity()
    {
        // Arrange
        var updatedVehicle = new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Green", 2019, "Refurbished");
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Vehicle>())).ReturnsAsync(updatedVehicle);

        // Act
        var result = await repositoryMock.Object.UpdateAsync(updatedVehicle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Green", result.Color);
    }

    [Fact(DisplayName = "UpdateAsync - Should Throw Exception When Update Fails")]
    public async Task Repository_UpdateAsync_ShouldThrowExceptionWhenUpdateFails()
    {
        // Arrange
        var updatedVehicle = new Vehicle(1, VehicleType.Car, "Ford", "Ka 1.0", "Green", 2019, "Refurbished");
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        var expectedErrorMessage = "Error when updating the record";
        repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Vehicle>())).ThrowsAsync(new Exception(expectedErrorMessage));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => repositoryMock.Object.UpdateAsync(updatedVehicle));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

    [Fact(DisplayName = "DeleteAsync - Should Complete Successfully")]
    public async Task Repository_DeleteAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        int entityId = 1;
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        repositoryMock.Setup(r => r.DeleteAsync(entityId)).Returns(Task.CompletedTask);

        // Act
        var exception = await Record.ExceptionAsync(async () => await repositoryMock.Object.DeleteAsync(entityId));

        // Assert
        Assert.Null(exception);
    }

    [Fact(DisplayName = "DeleteAsync - Should Throw Exception When Delete Fails")]
    public async Task Repository_DeleteAsync_ShouldThrowExceptionWhenDeleteFails()
    {
        // Arrange
        int entityId = 1;
        var repositoryMock = new Mock<IRepository<Vehicle>>();
        var expectedErrorMessage = "Error when deleting the record";
        repositoryMock.Setup(r => r.DeleteAsync(entityId)).ThrowsAsync(new Exception(expectedErrorMessage));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => repositoryMock.Object.DeleteAsync(entityId));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

}
