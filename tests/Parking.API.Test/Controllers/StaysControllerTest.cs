using Microsoft.AspNetCore.Mvc;
using Moq;
using Parking.Api.Controllers;
using Parking.API.Test.Helpers;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.API.Test.Controllers;

public class StaysControllerTest
{
    private readonly Mock<IStayService> _stayServiceMock;
    private readonly StaysController _controller;

    public StaysControllerTest()
    {
        _stayServiceMock = new Mock<IStayService>();
        _controller = new StaysController(_stayServiceMock.Object);
    }

    [Fact]
    public async Task StaysController_GetAll_ShouldReturnOkWithListOfStays()
    {
        // Arrange
        var stayList = new List<StayDTO> { StayTestHelper.CreateValidStayDTO(1) };

        _stayServiceMock
            .Setup(service => service.GetAllStays())
            .ReturnsAsync(stayList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedStays = Assert.IsAssignableFrom<IEnumerable<StayDTO>>(okResult.Value);
        Assert.Single(returnedStays);
        _stayServiceMock.Verify(service => service.GetAllStays(), Times.Once);
    }

    [Fact]
    public async Task StaysController_GetAll_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        _stayServiceMock
            .Setup(service => service.GetAllStays())
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAll());
        _stayServiceMock.Verify(service => service.GetAllStays(), Times.Once);
    }

    [Fact]
    public async Task StaysController_GetById_ShouldReturnOkWithStayWhenFound()
    {
        // Arrange
        const int stayId = 1;
        var stayDto = StayTestHelper.CreateValidStayDTO(stayId);

        _stayServiceMock
            .Setup(service => service.GetStayById(stayId))
            .ReturnsAsync(stayDto);

        // Act
        var result = await _controller.GetById(stayId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedStay = Assert.IsType<StayDTO>(okResult.Value);
        Assert.Equal(stayId, returnedStay.Id);
        _stayServiceMock.Verify(service => service.GetStayById(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_GetById_ShouldReturnNotFoundWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;

        _stayServiceMock
            .Setup(service => service.GetStayById(stayId))
            .ReturnsAsync((StayDTO)null!);

        // Act
        var result = await _controller.GetById(stayId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _stayServiceMock.Verify(service => service.GetStayById(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_GetById_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int stayId = 1;

        _stayServiceMock
            .Setup(service => service.GetStayById(stayId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetById(stayId));
        _stayServiceMock.Verify(service => service.GetStayById(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_Create_ShouldReturnCreatedAtActionWithCreatedStay()
    {
        // Arrange
        var inputDto = StayTestHelper.CreateValidStayDTO(0);
        var createdDto = StayTestHelper.CreateValidStayDTO(1);

        _stayServiceMock
            .Setup(service => service.CreateStay(inputDto))
            .ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(inputDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(StaysController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(createdDto.Id, createdAtActionResult.RouteValues!["id"]);

        var returnedStay = Assert.IsType<StayDTO>(createdAtActionResult.Value);
        Assert.Equal(createdDto.Id, returnedStay.Id);

        _stayServiceMock.Verify(service => service.CreateStay(inputDto), Times.Once);
    }

    [Fact]
    public async Task StaysController_Create_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        var inputDto = StayTestHelper.CreateValidStayDTO(0);

        _stayServiceMock
            .Setup(service => service.CreateStay(inputDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Create(inputDto));
        _stayServiceMock.Verify(service => service.CreateStay(inputDto), Times.Once);
    }

    [Fact]
    public async Task StaysController_Complete_ShouldReturnOkWithCompletedStay()
    {
        // Arrange
        const int stayId = 1;
        var exitDate = new DateTime(2026, 1, 1, 12, 0, 0);
        var completedStayDto = StayTestHelper.CreateValidStayDTO(stayId);
        completedStayDto.ExitDate = exitDate;
        completedStayDto.TotalAmount = 20.00m;

        _stayServiceMock
            .Setup(service => service.CompleteStay(stayId, exitDate))
            .ReturnsAsync(completedStayDto);

        // Act
        var result = await _controller.Complete(stayId, exitDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedStay = Assert.IsType<StayDTO>(okResult.Value);
        Assert.Equal(stayId, returnedStay.Id);
        Assert.Equal(exitDate, returnedStay.ExitDate);

        _stayServiceMock.Verify(service => service.CompleteStay(stayId, exitDate), Times.Once);
    }

    [Fact]
    public async Task StaysController_Complete_ShouldThrowKeyNotFoundExceptionWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;
        var exitDate = new DateTime(2026, 1, 1, 12, 0, 0);

        _stayServiceMock
            .Setup(service => service.CompleteStay(stayId, exitDate))
            .ThrowsAsync(new KeyNotFoundException($"Stay with ID {stayId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Complete(stayId, exitDate));
        _stayServiceMock.Verify(service => service.CompleteStay(stayId, exitDate), Times.Once);
    }

    [Fact]
    public async Task StaysController_Complete_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int stayId = 1;
        var exitDate = new DateTime(2026, 1, 1, 12, 0, 0);

        _stayServiceMock
            .Setup(service => service.CompleteStay(stayId, exitDate))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Complete(stayId, exitDate));
        _stayServiceMock.Verify(service => service.CompleteStay(stayId, exitDate), Times.Once);
    }

    [Fact]
    public async Task StaysController_Cancel_ShouldReturnOkWithCancelledStay()
    {
        // Arrange
        const int stayId = 1;
        var cancelledStayDto = StayTestHelper.CreateValidStayDTO(stayId);

        _stayServiceMock
            .Setup(service => service.CancelStay(stayId))
            .ReturnsAsync(cancelledStayDto);

        // Act
        var result = await _controller.Cancel(stayId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedStay = Assert.IsType<StayDTO>(okResult.Value);
        Assert.Equal(stayId, returnedStay.Id);

        _stayServiceMock.Verify(service => service.CancelStay(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_Cancel_ShouldThrowKeyNotFoundExceptionWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;

        _stayServiceMock
            .Setup(service => service.CancelStay(stayId))
            .ThrowsAsync(new KeyNotFoundException($"Stay with ID {stayId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Cancel(stayId));
        _stayServiceMock.Verify(service => service.CancelStay(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_Cancel_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int stayId = 1;

        _stayServiceMock
            .Setup(service => service.CancelStay(stayId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Cancel(stayId));
        _stayServiceMock.Verify(service => service.CancelStay(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_Delete_ShouldReturnNoContentWhenDeleteIsSuccessful()
    {
        // Arrange
        const int stayId = 1;

        _stayServiceMock
            .Setup(service => service.DeleteStay(stayId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(stayId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _stayServiceMock.Verify(service => service.DeleteStay(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_Delete_ShouldThrowKeyNotFoundExceptionWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;

        _stayServiceMock
            .Setup(service => service.DeleteStay(stayId))
            .ThrowsAsync(new KeyNotFoundException($"Stay with ID {stayId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Delete(stayId));
        _stayServiceMock.Verify(service => service.DeleteStay(stayId), Times.Once);
    }

    [Fact]
    public async Task StaysController_Delete_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int stayId = 1;

        _stayServiceMock
            .Setup(service => service.DeleteStay(stayId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Delete(stayId));
        _stayServiceMock.Verify(service => service.DeleteStay(stayId), Times.Once);
    }
}