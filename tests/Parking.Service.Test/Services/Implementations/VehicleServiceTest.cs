using MapsterMapper;
using Moq;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Implementations;
using Parking.Test.Shared.Helpers;

namespace Parking.Service.Test.Services.Implementations;

public class VehicleServiceTest
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly VehicleService _vehicleService;

    public VehicleServiceTest()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _mapperMock = new Mock<IMapper>();
        _vehicleService = new VehicleService(_vehicleRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task VehicleService_GetAllVehicles_ShouldReturnAllVehicles()
    {
        // Arrange
        var vehicleList = new List<Vehicle> { VehicleTestHelper.CreateValidVehicleEntity(1) };
        var vehicleDtoList = new List<VehicleDTO> { VehicleTestHelper.CreateValidVehicleDTO(1) };

        _vehicleRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(vehicleList);

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<VehicleDTO>>(vehicleList))
            .Returns(vehicleDtoList);

        // Act
        var result = await _vehicleService.GetAllVehicles();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _vehicleRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<VehicleDTO>>(vehicleList), Times.Once);
    }

    [Fact]
    public async Task VehicleService_GetAllVehicles_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        _vehicleRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _vehicleService.GetAllVehicles());
        _vehicleRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task VehicleService_GetVehicleById_ShouldReturnVehicleById()
    {
        // Arrange
        const int vehicleId = 1;
        var vehicleEntity = VehicleTestHelper.CreateValidVehicleEntity(vehicleId);
        var vehicleDto = VehicleTestHelper.CreateValidVehicleDTO(vehicleId);

        _vehicleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(vehicleId))
            .ReturnsAsync(vehicleEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<VehicleDTO>(vehicleEntity))
            .Returns(vehicleDto);

        // Act
        var result = await _vehicleService.GetVehicleById(vehicleId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicleId, result.Id);
        _vehicleRepositoryMock.Verify(repo => repo.GetByIdAsync(vehicleId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<VehicleDTO>(vehicleEntity), Times.Once);
    }

    [Fact]
    public async Task VehicleService_GetVehicleById_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        const int vehicleId = 999;

        _vehicleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(vehicleId))
            .ReturnsAsync((Vehicle)null!);

        _mapperMock
            .Setup(mapper => mapper.Map<VehicleDTO>(It.IsAny<Vehicle>()))
            .Returns((VehicleDTO)null!);

        // Act
        var result = await _vehicleService.GetVehicleById(vehicleId);

        // Assert
        Assert.Null(result);
        _vehicleRepositoryMock.Verify(repo => repo.GetByIdAsync(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehicleService_GetVehicleById_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int vehicleId = 1;

        _vehicleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(vehicleId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _vehicleService.GetVehicleById(vehicleId));
        _vehicleRepositoryMock.Verify(repo => repo.GetByIdAsync(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehicleService_CreateVehicle_ShouldCreateVehicle()
    {
        // Arrange
        var inputDto = VehicleTestHelper.CreateValidVehicleDTO(0);
        var mappedEntity = VehicleTestHelper.CreateValidVehicleEntity(0);
        var createdEntity = VehicleTestHelper.CreateValidVehicleEntity(1);
        var resultDto = VehicleTestHelper.CreateValidVehicleDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Vehicle>(inputDto))
            .Returns(mappedEntity);

        _vehicleRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ReturnsAsync(createdEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<VehicleDTO>(createdEntity))
            .Returns(resultDto);

        // Act
        var result = await _vehicleService.CreateVehicle(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<Vehicle>(inputDto), Times.Once);
        _vehicleRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<VehicleDTO>(createdEntity), Times.Once);
    }

    [Fact]
    public async Task VehicleService_CreateVehicle_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = VehicleTestHelper.CreateValidVehicleDTO(1);
        var mappedEntity = VehicleTestHelper.CreateValidVehicleEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Vehicle>(inputDto))
            .Returns(mappedEntity);

        _vehicleRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _vehicleService.CreateVehicle(inputDto));
        _vehicleRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
    }

    [Fact]
    public async Task VehicleService_UpdateVehicle_ShouldUpdateVehicle()
    {
        // Arrange
        var inputDto = VehicleTestHelper.CreateValidVehicleDTO(1);
        var mappedEntity = VehicleTestHelper.CreateValidVehicleEntity(1);
        var updatedEntity = VehicleTestHelper.CreateValidVehicleEntity(1);
        var resultDto = VehicleTestHelper.CreateValidVehicleDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Vehicle>(inputDto))
            .Returns(mappedEntity);

        _vehicleRepositoryMock
            .Setup(repo => repo.UpdateAsync(mappedEntity))
            .ReturnsAsync(updatedEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<VehicleDTO>(updatedEntity))
            .Returns(resultDto);

        // Act
        var result = await _vehicleService.UpdateVehicle(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inputDto.Id, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<Vehicle>(inputDto), Times.Once);
        _vehicleRepositoryMock.Verify(repo => repo.UpdateAsync(mappedEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<VehicleDTO>(updatedEntity), Times.Once);
    }

    [Fact]
    public async Task VehicleService_UpdateVehicle_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = VehicleTestHelper.CreateValidVehicleDTO(1);
        var mappedEntity = VehicleTestHelper.CreateValidVehicleEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Vehicle>(inputDto))
            .Returns(mappedEntity);

        _vehicleRepositoryMock
            .Setup(repo => repo.UpdateAsync(mappedEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _vehicleService.UpdateVehicle(inputDto));
        _vehicleRepositoryMock.Verify(repo => repo.UpdateAsync(mappedEntity), Times.Once);
    }

    [Fact]
    public async Task VehicleService_DeleteVehicle_ShouldDeleteVehicle()
    {
        // Arrange
        const int vehicleId = 1;

        _vehicleRepositoryMock
            .Setup(repo => repo.DeleteAsync(vehicleId))
            .Returns(Task.CompletedTask);

        // Act
        await _vehicleService.DeleteVehicle(vehicleId);

        // Assert
        _vehicleRepositoryMock.Verify(repo => repo.DeleteAsync(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehicleService_DeleteVehicle_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int vehicleId = 1;

        _vehicleRepositoryMock
            .Setup(repo => repo.DeleteAsync(vehicleId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _vehicleService.DeleteVehicle(vehicleId));
        _vehicleRepositoryMock.Verify(repo => repo.DeleteAsync(vehicleId), Times.Once);
    }
}