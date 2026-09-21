using MapsterMapper;
using Moq;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Implementations;
using Parking.Test.Shared.Helpers;

namespace Parking.Service.Test.Services.Implementations;

public class CustomerServiceTest
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CustomerService _customerService;

    public CustomerServiceTest()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CustomerService_GetAllCustomers_ShouldReturnAllCustomers()
    {
        // Arrange
        var customerList = new List<Customer> { CustomerTestHelper.CreateValidCustomerEntity(1) };
        var customerDtoList = new List<CustomerDTO> { CustomerTestHelper.CreateValidCustomerDTO(1) };

        _customerRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(customerList);

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<CustomerDTO>>(customerList))
            .Returns(customerDtoList);

        // Act
        var result = await _customerService.GetAllCustomers();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _customerRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<CustomerDTO>>(customerList), Times.Once);
    }

    [Fact]
    public async Task CustomerService_GetAllCustomers_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.GetAllCustomers());
        _customerRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CustomerService_GetCustomerById_ShouldReturnCustomerById()
    {
        // Arrange
        const int customerId = 1;
        var customerEntity = CustomerTestHelper.CreateValidCustomerEntity(customerId);
        var customerDto = CustomerTestHelper.CreateValidCustomerDTO(customerId);

        _customerRepositoryMock
            .Setup(repo => repo.GetByIdAsync(customerId))
            .ReturnsAsync(customerEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerDTO>(customerEntity))
            .Returns(customerDto);

        // Act
        var result = await _customerService.GetCustomerById(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
        _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CustomerDTO>(customerEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerService_GetCustomerById_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        const int customerId = 999;

        _customerRepositoryMock
            .Setup(repo => repo.GetByIdAsync(customerId))
            .ReturnsAsync((Customer)null!);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerDTO>(It.IsAny<Customer>()))
            .Returns((CustomerDTO)null!);

        // Act
        var result = await _customerService.GetCustomerById(customerId);

        // Assert
        Assert.Null(result);
        _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomerService_GetCustomerById_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int customerId = 1;

        _customerRepositoryMock
            .Setup(repo => repo.GetByIdAsync(customerId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.GetCustomerById(customerId));
        _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomerService_CreateCustomer_ShouldCreateCustomer()
    {
        // Arrange
        var inputDto = CustomerTestHelper.CreateValidCustomerDTO(0);
        var mappedEntity = CustomerTestHelper.CreateValidCustomerEntity(0);
        var createdEntity = CustomerTestHelper.CreateValidCustomerEntity(1);
        var fullEntity = CustomerTestHelper.CreateValidCustomerEntity(1);
        var resultDto = CustomerTestHelper.CreateValidCustomerDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Customer>(inputDto))
            .Returns(mappedEntity);

        _customerRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(createdEntity);

        _customerRepositoryMock
            .Setup(repo => repo.GetByIdAsync(createdEntity.Id))
            .ReturnsAsync(fullEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerDTO>(fullEntity))
            .Returns(resultDto);

        // Act
        var result = await _customerService.CreateCustomer(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<Customer>(inputDto), Times.Once);
        _customerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
        _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(createdEntity.Id), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CustomerDTO>(fullEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerService_CreateCustomer_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = CustomerTestHelper.CreateValidCustomerDTO(1);
        var mappedEntity = CustomerTestHelper.CreateValidCustomerEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Customer>(inputDto))
            .Returns(mappedEntity);

        _customerRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.CreateCustomer(inputDto));
        _customerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task CustomerService_UpdateCustomer_ShouldUpdateCustomer()
    {
        // Arrange
        var inputDto = CustomerTestHelper.CreateValidCustomerDTO(1);
        var mappedEntity = CustomerTestHelper.CreateValidCustomerEntity(1);
        var updatedEntity = CustomerTestHelper.CreateValidCustomerEntity(1);
        var resultDto = CustomerTestHelper.CreateValidCustomerDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Customer>(inputDto))
            .Returns(mappedEntity);

        _customerRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(updatedEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<CustomerDTO>(updatedEntity))
            .Returns(resultDto);

        // Act
        var result = await _customerService.UpdateCustomer(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inputDto.Id, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<Customer>(inputDto), Times.Once);
        _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Customer>()), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CustomerDTO>(updatedEntity), Times.Once);
    }

    [Fact]
    public async Task CustomerService_UpdateCustomer_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = CustomerTestHelper.CreateValidCustomerDTO(1);
        var mappedEntity = CustomerTestHelper.CreateValidCustomerEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Customer>(inputDto))
            .Returns(mappedEntity);

        _customerRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Customer>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.UpdateCustomer(inputDto));
        _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task CustomerService_DeleteCustomer_ShouldDeleteCustomer()
    {
        // Arrange
        const int customerId = 1;

        _customerRepositoryMock
            .Setup(repo => repo.DeleteAsync(customerId))
            .Returns(Task.CompletedTask);

        // Act
        await _customerService.DeleteCustomer(customerId);

        // Assert
        _customerRepositoryMock.Verify(repo => repo.DeleteAsync(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomerService_DeleteCustomer_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int customerId = 1;

        _customerRepositoryMock
            .Setup(repo => repo.DeleteAsync(customerId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.DeleteCustomer(customerId));
        _customerRepositoryMock.Verify(repo => repo.DeleteAsync(customerId), Times.Once);
    }
}