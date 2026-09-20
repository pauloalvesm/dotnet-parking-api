using Microsoft.AspNetCore.Mvc;
using Moq;
using Parking.Api.Controllers;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;
using Parking.Test.Shared.Helpers;

namespace Parking.API.Test.Controllers;

public class CustomersControllerTest
{
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly CustomersController _controller;

    public CustomersControllerTest()
    {
        _customerServiceMock = new Mock<ICustomerService>();
        _controller = new CustomersController(_customerServiceMock.Object);
    }

    [Fact]
    public async Task CustomersController_GetAll_ShouldReturnOkWithListOfCustomers()
    {
        // Arrange
        var customerList = new List<CustomerDTO> { CustomerTestHelper.CreateValidCustomerDTO(1) };

        _customerServiceMock
            .Setup(service => service.GetAllCustomers())
            .ReturnsAsync(customerList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<CustomerDTO>>(okResult.Value);
        Assert.Single(returnedCustomers);
        _customerServiceMock.Verify(service => service.GetAllCustomers(), Times.Once);
    }

    [Fact]
    public async Task CustomersController_GetAll_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        _customerServiceMock
            .Setup(service => service.GetAllCustomers())
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAll());
        _customerServiceMock.Verify(service => service.GetAllCustomers(), Times.Once);
    }

    [Fact]
    public async Task CustomersController_GetById_ShouldReturnOkWithCustomerWhenFound()
    {
        // Arrange
        const int customerId = 1;
        var customerDto = CustomerTestHelper.CreateValidCustomerDTO(customerId);

        _customerServiceMock
            .Setup(service => service.GetCustomerById(customerId))
            .ReturnsAsync(customerDto);

        // Act
        var result = await _controller.GetById(customerId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCustomer = Assert.IsType<CustomerDTO>(okResult.Value);
        Assert.Equal(customerId, returnedCustomer.Id);
        _customerServiceMock.Verify(service => service.GetCustomerById(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomersController_GetById_ShouldReturnNotFoundWhenCustomerDoesNotExist()
    {
        // Arrange
        const int customerId = 999;

        _customerServiceMock
            .Setup(service => service.GetCustomerById(customerId))
            .ReturnsAsync((CustomerDTO)null!);

        // Act
        var result = await _controller.GetById(customerId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _customerServiceMock.Verify(service => service.GetCustomerById(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomersController_GetById_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int customerId = 1;

        _customerServiceMock
            .Setup(service => service.GetCustomerById(customerId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetById(customerId));
        _customerServiceMock.Verify(service => service.GetCustomerById(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Create_ShouldReturnCreatedAtActionWithCreatedCustomer()
    {
        // Arrange
        var inputDto = CustomerTestHelper.CreateValidCustomerDTO(0);
        var createdDto = CustomerTestHelper.CreateValidCustomerDTO(1);

        _customerServiceMock
            .Setup(service => service.CreateCustomer(inputDto))
            .ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(inputDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(CustomersController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(createdDto.Id, createdAtActionResult.RouteValues!["id"]);

        var returnedCustomer = Assert.IsType<CustomerDTO>(createdAtActionResult.Value);
        Assert.Equal(createdDto.Id, returnedCustomer.Id);

        _customerServiceMock.Verify(service => service.CreateCustomer(inputDto), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Create_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        var inputDto = CustomerTestHelper.CreateValidCustomerDTO(0);

        _customerServiceMock
            .Setup(service => service.CreateCustomer(inputDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Create(inputDto));
        _customerServiceMock.Verify(service => service.CreateCustomer(inputDto), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Update_ShouldReturnOkWithUpdatedCustomerWhenIdsMatch()
    {
        // Arrange
        const int customerId = 1;
        var customerDto = CustomerTestHelper.CreateValidCustomerDTO(customerId);

        _customerServiceMock
            .Setup(service => service.UpdateCustomer(customerDto))
            .ReturnsAsync(customerDto);

        // Act
        var result = await _controller.Update(customerId, customerDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCustomer = Assert.IsType<CustomerDTO>(okResult.Value);
        Assert.Equal(customerId, returnedCustomer.Id);

        _customerServiceMock.Verify(service => service.UpdateCustomer(customerDto), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Update_ShouldReturnBadRequestWhenIdMismatch()
    {
        // Arrange
        const int routeId = 1;
        var customerDto = CustomerTestHelper.CreateValidCustomerDTO(2);

        // Act
        var result = await _controller.Update(routeId, customerDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("ID Mismatch", badRequestResult.Value);

        _customerServiceMock.Verify(service => service.UpdateCustomer(It.IsAny<CustomerDTO>()), Times.Never);
    }

    [Fact]
    public async Task CustomersController_Update_ShouldThrowKeyNotFoundExceptionWhenCustomerDoesNotExistInService()
    {
        // Arrange
        const int customerId = 999;
        var customerDto = CustomerTestHelper.CreateValidCustomerDTO(customerId);

        _customerServiceMock
            .Setup(service => service.UpdateCustomer(customerDto))
            .ThrowsAsync(new KeyNotFoundException($"Customer with ID {customerId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Update(customerId, customerDto));
        _customerServiceMock.Verify(service => service.UpdateCustomer(customerDto), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Update_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int customerId = 1;
        var customerDto = CustomerTestHelper.CreateValidCustomerDTO(customerId);

        _customerServiceMock
            .Setup(service => service.UpdateCustomer(customerDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Update(customerId, customerDto));
        _customerServiceMock.Verify(service => service.UpdateCustomer(customerDto), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Delete_ShouldReturnNoContentWhenDeleteIsSuccessful()
    {
        // Arrange
        const int customerId = 1;

        _customerServiceMock
            .Setup(service => service.DeleteCustomer(customerId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(customerId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _customerServiceMock.Verify(service => service.DeleteCustomer(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Delete_ShouldThrowKeyNotFoundExceptionWhenCustomerDoesNotExist()
    {
        // Arrange
        const int customerId = 999;

        _customerServiceMock
            .Setup(service => service.DeleteCustomer(customerId))
            .ThrowsAsync(new KeyNotFoundException($"Customer with ID {customerId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Delete(customerId));
        _customerServiceMock.Verify(service => service.DeleteCustomer(customerId), Times.Once);
    }

    [Fact]
    public async Task CustomersController_Delete_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int customerId = 1;

        _customerServiceMock
            .Setup(service => service.DeleteCustomer(customerId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Delete(customerId));
        _customerServiceMock.Verify(service => service.DeleteCustomer(customerId), Times.Once);
    }
}