using Microsoft.AspNetCore.Mvc;
using Moq;
using Parking.Api.Controllers;
using Parking.API.Test.Helpers;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;
using Xunit;

namespace Parking.API.Test.Controllers;

public class CustomerVehiclesControllerTest
{
    private readonly Mock<ICustomerVehicleService> _customerVehicleServiceMock;
    private readonly CustomerVehiclesController _controller;

    public CustomerVehiclesControllerTest()
    {
        _customerVehicleServiceMock = new Mock<ICustomerVehicleService>();
        _controller = new CustomerVehiclesController(_customerVehicleServiceMock.Object);
    }

    [Fact]
    public async Task CustomerVehiclesController_GetAll_ShouldReturnOkWithListOfCustomerVehicles()
    {
        // Arrange
        var customerVehicleList = new List<CustomerVehicleDTO>
        {
            CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1)
        };

        _customerVehicleServiceMock
            .Setup(service => service.GetAllCustomerVehicles())
            .ReturnsAsync(customerVehicleList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedItems = Assert.IsAssignableFrom<IEnumerable<CustomerVehicleDTO>>(okResult.Value);
        Assert.Single(returnedItems);
        _customerVehicleServiceMock.Verify(service => service.GetAllCustomerVehicles(), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_GetAll_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        _customerVehicleServiceMock
            .Setup(service => service.GetAllCustomerVehicles())
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAll());
        _customerVehicleServiceMock.Verify(service => service.GetAllCustomerVehicles(), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_GetById_ShouldReturnOkWithCustomerVehicleWhenFound()
    {
        // Arrange
        const int customerVehicleId = 1;
        var customerVehicleDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(customerVehicleId);

        _customerVehicleServiceMock
            .Setup(service => service.GetCustomerVehicleById(customerVehicleId))
            .ReturnsAsync(customerVehicleDto);

        // Act
        var result = await _controller.GetById(customerVehicleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedItem = Assert.IsType<CustomerVehicleDTO>(okResult.Value);
        Assert.Equal(customerVehicleId, returnedItem.Id);
        _customerVehicleServiceMock.Verify(service => service.GetCustomerVehicleById(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_GetById_ShouldReturnNotFoundWhenCustomerVehicleDoesNotExist()
    {
        // Arrange
        const int customerVehicleId = 999;

        _customerVehicleServiceMock
            .Setup(service => service.GetCustomerVehicleById(customerVehicleId))
            .ReturnsAsync((CustomerVehicleDTO)null!);

        // Act
        var result = await _controller.GetById(customerVehicleId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _customerVehicleServiceMock.Verify(service => service.GetCustomerVehicleById(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_GetById_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int customerVehicleId = 1;

        _customerVehicleServiceMock
            .Setup(service => service.GetCustomerVehicleById(customerVehicleId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetById(customerVehicleId));
        _customerVehicleServiceMock.Verify(service => service.GetCustomerVehicleById(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Create_ShouldReturnCreatedAtActionWithCreatedCustomerVehicle()
    {
        // Arrange
        var inputDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(0);
        var createdDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1);

        _customerVehicleServiceMock
            .Setup(service => service.CreateCustomerVehicle(inputDto))
            .ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(inputDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(CustomerVehiclesController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(createdDto.Id, createdAtActionResult.RouteValues!["id"]);

        var returnedItem = Assert.IsType<CustomerVehicleDTO>(createdAtActionResult.Value);
        Assert.Equal(createdDto.Id, returnedItem.Id);

        _customerVehicleServiceMock.Verify(service => service.CreateCustomerVehicle(inputDto), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Create_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        var inputDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(0);

        _customerVehicleServiceMock
            .Setup(service => service.CreateCustomerVehicle(inputDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Create(inputDto));
        _customerVehicleServiceMock.Verify(service => service.CreateCustomerVehicle(inputDto), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Update_ShouldReturnOkWithUpdatedCustomerVehicleWhenIdsMatch()
    {
        // Arrange
        const int customerVehicleId = 1;
        var customerVehicleDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(customerVehicleId);

        _customerVehicleServiceMock
            .Setup(service => service.UpdateCustomerVehicle(customerVehicleDto))
            .ReturnsAsync(customerVehicleDto);

        // Act
        var result = await _controller.Update(customerVehicleId, customerVehicleDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedItem = Assert.IsType<CustomerVehicleDTO>(okResult.Value);
        Assert.Equal(customerVehicleId, returnedItem.Id);

        _customerVehicleServiceMock.Verify(service => service.UpdateCustomerVehicle(customerVehicleDto), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Update_ShouldReturnBadRequestWhenIdMismatch()
    {
        // Arrange
        const int routeId = 1;
        var customerVehicleDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(2);

        // Act
        var result = await _controller.Update(routeId, customerVehicleDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("ID Mismatch", badRequestResult.Value);

        _customerVehicleServiceMock.Verify(service => service.UpdateCustomerVehicle(It.IsAny<CustomerVehicleDTO>()), Times.Never);
    }

    [Fact]
    public async Task CustomerVehiclesController_Update_ShouldThrowKeyNotFoundExceptionWhenCustomerVehicleDoesNotExistInService()
    {
        // Arrange
        const int customerVehicleId = 999;
        var customerVehicleDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(customerVehicleId);

        _customerVehicleServiceMock
            .Setup(service => service.UpdateCustomerVehicle(customerVehicleDto))
            .ThrowsAsync(new KeyNotFoundException($"CustomerVehicle with ID {customerVehicleId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Update(customerVehicleId, customerVehicleDto));
        _customerVehicleServiceMock.Verify(service => service.UpdateCustomerVehicle(customerVehicleDto), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Update_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int customerVehicleId = 1;
        var customerVehicleDto = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(customerVehicleId);

        _customerVehicleServiceMock
            .Setup(service => service.UpdateCustomerVehicle(customerVehicleDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Update(customerVehicleId, customerVehicleDto));
        _customerVehicleServiceMock.Verify(service => service.UpdateCustomerVehicle(customerVehicleDto), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Delete_ShouldReturnNoContentWhenDeleteIsSuccessful()
    {
        // Arrange
        const int customerVehicleId = 1;

        _customerVehicleServiceMock
            .Setup(service => service.DeleteCustomerVehicle(customerVehicleId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(customerVehicleId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _customerVehicleServiceMock.Verify(service => service.DeleteCustomerVehicle(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Delete_ShouldThrowKeyNotFoundExceptionWhenCustomerVehicleDoesNotExist()
    {
        // Arrange
        const int customerVehicleId = 999;

        _customerVehicleServiceMock
            .Setup(service => service.DeleteCustomerVehicle(customerVehicleId))
            .ThrowsAsync(new KeyNotFoundException($"CustomerVehicle with ID {customerVehicleId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Delete(customerVehicleId));
        _customerVehicleServiceMock.Verify(service => service.DeleteCustomerVehicle(customerVehicleId), Times.Once);
    }

    [Fact]
    public async Task CustomerVehiclesController_Delete_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int customerVehicleId = 1;

        _customerVehicleServiceMock
            .Setup(service => service.DeleteCustomerVehicle(customerVehicleId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Delete(customerVehicleId));
        _customerVehicleServiceMock.Verify(service => service.DeleteCustomerVehicle(customerVehicleId), Times.Once);
    }
}