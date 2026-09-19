using Microsoft.AspNetCore.Mvc;
using Moq;
using Parking.Api.Controllers;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;
using Parking.Test.Shared.Helpers;

namespace Parking.API.Test.Controllers;

public class VehiclesControllerTest
{
    private readonly Mock<IVehicleService> _vehicleServiceMock;
    private readonly VehiclesController _controller;

    public VehiclesControllerTest()
    {
        _vehicleServiceMock = new Mock<IVehicleService>();
        _controller = new VehiclesController(_vehicleServiceMock.Object);
    }

    [Fact]
    public async Task VehiclesController_GetAll_ShouldReturnOkWithListOfVehicles()
    {
        // Arrange
        var vehicleList = new List<VehicleDTO> { VehicleTestHelper.CreateValidVehicleDTO(1) };

        _vehicleServiceMock
            .Setup(service => service.GetAllVehicles())
            .ReturnsAsync(vehicleList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedVehicles = Assert.IsAssignableFrom<IEnumerable<VehicleDTO>>(okResult.Value);
        Assert.Single(returnedVehicles);
        _vehicleServiceMock.Verify(service => service.GetAllVehicles(), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_GetAll_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        _vehicleServiceMock
            .Setup(service => service.GetAllVehicles())
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAll());
        _vehicleServiceMock.Verify(service => service.GetAllVehicles(), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_GetById_ShouldReturnOkWithVehicleWhenFound()
    {
        // Arrange
        const int vehicleId = 1;
        var vehicleDto = VehicleTestHelper.CreateValidVehicleDTO(vehicleId);

        _vehicleServiceMock
            .Setup(service => service.GetVehicleById(vehicleId))
            .ReturnsAsync(vehicleDto);

        // Act
        var result = await _controller.GetById(vehicleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedVehicle = Assert.IsType<VehicleDTO>(okResult.Value);
        Assert.Equal(vehicleId, returnedVehicle.Id);
        _vehicleServiceMock.Verify(service => service.GetVehicleById(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_GetById_ShouldReturnNotFoundWhenVehicleDoesNotExist()
    {
        // Arrange
        const int vehicleId = 999;

        _vehicleServiceMock
            .Setup(service => service.GetVehicleById(vehicleId))
            .ReturnsAsync((VehicleDTO)null!);

        // Act
        var result = await _controller.GetById(vehicleId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _vehicleServiceMock.Verify(service => service.GetVehicleById(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_GetById_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int vehicleId = 1;

        _vehicleServiceMock
            .Setup(service => service.GetVehicleById(vehicleId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetById(vehicleId));
        _vehicleServiceMock.Verify(service => service.GetVehicleById(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Create_ShouldReturnCreatedAtActionWithCreatedVehicle()
    {
        // Arrange
        var inputDto = VehicleTestHelper.CreateValidVehicleDTO(0);
        var createdDto = VehicleTestHelper.CreateValidVehicleDTO(1);

        _vehicleServiceMock
            .Setup(service => service.CreateVehicle(inputDto))
            .ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(inputDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(VehiclesController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(createdDto.Id, createdAtActionResult.RouteValues!["id"]);

        var returnedVehicle = Assert.IsType<VehicleDTO>(createdAtActionResult.Value);
        Assert.Equal(createdDto.Id, returnedVehicle.Id);

        _vehicleServiceMock.Verify(service => service.CreateVehicle(inputDto), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Create_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        var inputDto = VehicleTestHelper.CreateValidVehicleDTO(0);

        _vehicleServiceMock
            .Setup(service => service.CreateVehicle(inputDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Create(inputDto));
        _vehicleServiceMock.Verify(service => service.CreateVehicle(inputDto), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Update_ShouldReturnOkWithUpdatedVehicleWhenIdsMatch()
    {
        // Arrange
        const int vehicleId = 1;
        var vehicleDto = VehicleTestHelper.CreateValidVehicleDTO(vehicleId);

        _vehicleServiceMock
            .Setup(service => service.UpdateVehicle(vehicleDto))
            .ReturnsAsync(vehicleDto);

        // Act
        var result = await _controller.Update(vehicleId, vehicleDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedVehicle = Assert.IsType<VehicleDTO>(okResult.Value);
        Assert.Equal(vehicleId, returnedVehicle.Id);

        _vehicleServiceMock.Verify(service => service.UpdateVehicle(vehicleDto), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Update_ShouldReturnBadRequestWhenIdMismatch()
    {
        // Arrange
        const int routeId = 1;
        var vehicleDto = VehicleTestHelper.CreateValidVehicleDTO(2);

        // Act
        var result = await _controller.Update(routeId, vehicleDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("ID Mismatch", badRequestResult.Value);

        _vehicleServiceMock.Verify(service => service.UpdateVehicle(It.IsAny<VehicleDTO>()), Times.Never);
    }

    [Fact]
    public async Task VehiclesController_Update_ShouldThrowKeyNotFoundExceptionWhenVehicleDoesNotExistInService()
    {
        // Arrange
        const int vehicleId = 999;
        var vehicleDto = VehicleTestHelper.CreateValidVehicleDTO(vehicleId);

        _vehicleServiceMock
            .Setup(service => service.UpdateVehicle(vehicleDto))
            .ThrowsAsync(new KeyNotFoundException($"Vehicle with ID {vehicleId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Update(vehicleId, vehicleDto));
        _vehicleServiceMock.Verify(service => service.UpdateVehicle(vehicleDto), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Update_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int vehicleId = 1;
        var vehicleDto = VehicleTestHelper.CreateValidVehicleDTO(vehicleId);

        _vehicleServiceMock
            .Setup(service => service.UpdateVehicle(vehicleDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Update(vehicleId, vehicleDto));
        _vehicleServiceMock.Verify(service => service.UpdateVehicle(vehicleDto), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Delete_ShouldReturnNoContentWhenDeleteIsSuccessful()
    {
        // Arrange
        const int vehicleId = 1;

        _vehicleServiceMock
            .Setup(service => service.DeleteVehicle(vehicleId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(vehicleId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _vehicleServiceMock.Verify(service => service.DeleteVehicle(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Delete_ShouldThrowKeyNotFoundExceptionWhenVehicleDoesNotExist()
    {
        // Arrange
        const int vehicleId = 999;

        _vehicleServiceMock
            .Setup(service => service.DeleteVehicle(vehicleId))
            .ThrowsAsync(new KeyNotFoundException($"Vehicle with ID {vehicleId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Delete(vehicleId));
        _vehicleServiceMock.Verify(service => service.DeleteVehicle(vehicleId), Times.Once);
    }

    [Fact]
    public async Task VehiclesController_Delete_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int vehicleId = 1;

        _vehicleServiceMock
            .Setup(service => service.DeleteVehicle(vehicleId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Delete(vehicleId));
        _vehicleServiceMock.Verify(service => service.DeleteVehicle(vehicleId), Times.Once);
    }
}