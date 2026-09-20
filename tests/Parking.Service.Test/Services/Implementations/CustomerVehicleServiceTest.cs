using MapsterMapper;
using Moq;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Implementations;
using Parking.Test.Shared.Helpers;

namespace Parking.Service.Test.Services.Implementations;

public class CustomerVehicleServiceTest
{
    private readonly Mock<ICustomerVehicleRepository> _customerVehicleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CustomerVehicleService _customerVehicleService;

    public CustomerVehicleServiceTest()
    {
        _customerVehicleRepositoryMock = new Mock<ICustomerVehicleRepository>();
        _mapperMock = new Mock<IMapper>();
        _customerVehicleService = new CustomerVehicleService(_customerVehicleRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CustomerVehicleService_GetAllCustomerVehicles_ShouldReturnAllCustomerVehicles()
    {
        // Arrange
        var customerVehicleList = new List<CustomerVehicle> { CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(1) };
        var customerVehicleDtoList = new List<CustomerVehicleDTO> { CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1) };

        _customerVehicleRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(customerVehicleList);

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<CustomerVehicleDTO>>(customerVehicleList))
            .Returns(customerVehicleDtoList);

        // Act
        var result = await _customerVehicleService.GetAllCustomerVehicles();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _customerVehicleRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<CustomerVehicleDTO>>(customerVehicleList), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_GetAllCustomerVehicles_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        _customerVehicleRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerVehicleService.GetAllCustomerVehicles());
        _customerVehicleRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_GetCustomerVehicleById_ShouldReturnCustomerVehicleById()
    {
        // Arrange
        const int customerVehicleId = 1;
        var customerVehicleEntity = CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(customerVehicleId);
        var customerVehicleDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(customerVehicleId);

        _customerVehicleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(customerVehicleId))
            .ReturnsAsync(customerVehicleEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicleDTO>(customerVehicleEntity))
            .Returns(customerVehicleDto);

        // Act
        var result = await _customerVehicleService.GetCustomerVehicleById(customerVehicleId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerVehicleId, result.Id);
        _customerVehicleRepositoryMock.Verify(repo => repo.GetByIdAsync(customerVehicleId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CustomerVehicleDTO>(customerVehicleEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_GetCustomerVehicleById_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        const int customerVehicleId = 999;

        _customerVehicleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(customerVehicleId))
            .ReturnsAsync((CustomerVehicle)null!);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicleDTO>(It.IsAny<CustomerVehicle>()))
            .Returns((CustomerVehicleDTO)null!);

        // Act
        var result = await _customerVehicleService.GetCustomerVehicleById(customerVehicleId);

        // Assert
        Assert.Null(result);
        _customerVehicleRepositoryMock.Verify(repo => repo.GetByIdAsync(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_GetCustomerVehicleById_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int customerVehicleId = 1;

        _customerVehicleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(customerVehicleId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerVehicleService.GetCustomerVehicleById(customerVehicleId));
        _customerVehicleRepositoryMock.Verify(repo => repo.GetByIdAsync(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_CreateCustomerVehicle_ShouldCreateCustomerVehicle()
    {
        // Arrange
        var inputDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(0);
        var mappedEntity = CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(0);
        var createdEntity = CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(1);
        var resultDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicle>(inputDto))
            .Returns(mappedEntity);

        _customerVehicleRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ReturnsAsync(createdEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicleDTO>(createdEntity))
            .Returns(resultDto);

        // Act
        var result = await _customerVehicleService.CreateCustomerVehicle(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<CustomerVehicle>(inputDto), Times.Once);
        _customerVehicleRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CustomerVehicleDTO>(createdEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_CreateCustomerVehicle_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1);
        var mappedEntity = CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicle>(inputDto))
            .Returns(mappedEntity);

        _customerVehicleRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerVehicleService.CreateCustomerVehicle(inputDto));
        _customerVehicleRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_UpdateCustomerVehicle_ShouldUpdateCustomerVehicle()
    {
        // Arrange
        var inputDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1);
        var mappedEntity = CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(1);
        var updatedEntity = CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(1);
        var resultDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicle>(inputDto))
            .Returns(mappedEntity);

        _customerVehicleRepositoryMock
            .Setup(repo => repo.UpdateAsync(mappedEntity))
            .ReturnsAsync(updatedEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicleDTO>(updatedEntity))
            .Returns(resultDto);

        // Act
        var result = await _customerVehicleService.UpdateCustomerVehicle(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inputDto.Id, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<CustomerVehicle>(inputDto), Times.Once);
        _customerVehicleRepositoryMock.Verify(repo => repo.UpdateAsync(mappedEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CustomerVehicleDTO>(updatedEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_UpdateCustomerVehicle_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1);
        var mappedEntity = CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerVehicle>(inputDto))
            .Returns(mappedEntity);

        _customerVehicleRepositoryMock
            .Setup(repo => repo.UpdateAsync(mappedEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerVehicleService.UpdateCustomerVehicle(inputDto));
        _customerVehicleRepositoryMock.Verify(repo => repo.UpdateAsync(mappedEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_DeleteCustomerVehicle_ShouldDeleteCustomerVehicle()
    {
        // Arrange
        const int customerVehicleId = 1;

        _customerVehicleRepositoryMock
            .Setup(repo => repo.DeleteAsync(customerVehicleId))
            .Returns(Task.CompletedTask);

        // Act
        await _customerVehicleService.DeleteCustomerVehicle(customerVehicleId);

        // Assert
        _customerVehicleRepositoryMock.Verify(repo => repo.DeleteAsync(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehicleService_DeleteCustomerVehicle_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int customerVehicleId = 1;

        _customerVehicleRepositoryMock
            .Setup(repo => repo.DeleteAsync(customerVehicleId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerVehicleService.DeleteCustomerVehicle(customerVehicleId));
        _customerVehicleRepositoryMock.Verify(repo => repo.DeleteAsync(customerVehicleId), Times.Once);
    }
}