using Microsoft.AspNetCore.Mvc;
using Moq;
using Parking.Api.Controllers;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;
using Parking.Test.Shared.Helpers;

namespace Parking.API.Test.Controllers;

public class AddressesControllerTest
{
    private readonly Mock<IAddressService> _addressServiceMock;
    private readonly AddressesController _controller;

    public AddressesControllerTest()
    {
        _addressServiceMock = new Mock<IAddressService>();
        _controller = new AddressesController(_addressServiceMock.Object);
    }

    [Fact]
    public async Task AddressesController_GetAll_ShouldReturnOkWithListOfAddresses()
    {
        // Arrange
        var addressList = new List<AddressDTO> { AddressTestHelper.CreateValidAddressDTO(1) };

        _addressServiceMock
            .Setup(service => service.GetAllAddresses())
            .ReturnsAsync(addressList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedAddresses = Assert.IsAssignableFrom<IEnumerable<AddressDTO>>(okResult.Value);
        Assert.Single(returnedAddresses);
        _addressServiceMock.Verify(service => service.GetAllAddresses(), Times.Once);
    }

    [Fact]
    public async Task AddressesController_GetAll_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        _addressServiceMock
            .Setup(service => service.GetAllAddresses())
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAll());
        _addressServiceMock.Verify(service => service.GetAllAddresses(), Times.Once);
    }

    [Fact]
    public async Task AddressesController_GetById_ShouldReturnOkWithAddressWhenFound()
    {
        // Arrange
        const int addressId = 1;
        var addressDto = AddressTestHelper.CreateValidAddressDTO(addressId);

        _addressServiceMock
            .Setup(service => service.GetAddressById(addressId))
            .ReturnsAsync(addressDto);

        // Act
        var result = await _controller.GetById(addressId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedAddress = Assert.IsType<AddressDTO>(okResult.Value);
        Assert.Equal(addressId, returnedAddress.Id);
        _addressServiceMock.Verify(service => service.GetAddressById(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressesController_GetById_ShouldReturnNotFoundWhenAddressDoesNotExist()
    {
        // Arrange
        const int addressId = 999;

        _addressServiceMock
            .Setup(service => service.GetAddressById(addressId))
            .ReturnsAsync((AddressDTO)null!);

        // Act
        var result = await _controller.GetById(addressId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _addressServiceMock.Verify(service => service.GetAddressById(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressesController_GetById_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int addressId = 1;

        _addressServiceMock
            .Setup(service => service.GetAddressById(addressId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetById(addressId));
        _addressServiceMock.Verify(service => service.GetAddressById(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Create_ShouldReturnCreatedAtActionWithCreatedAddress()
    {
        // Arrange
        var inputDto = AddressTestHelper.CreateValidAddressDTO(0);
        var createdDto = AddressTestHelper.CreateValidAddressDTO(1);

        _addressServiceMock
            .Setup(service => service.CreateAddress(inputDto))
            .ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(inputDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(AddressesController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(createdDto.Id, createdAtActionResult.RouteValues!["id"]);

        var returnedAddress = Assert.IsType<AddressDTO>(createdAtActionResult.Value);
        Assert.Equal(createdDto.Id, returnedAddress.Id);

        _addressServiceMock.Verify(service => service.CreateAddress(inputDto), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Create_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        var inputDto = AddressTestHelper.CreateValidAddressDTO(0);

        _addressServiceMock
            .Setup(service => service.CreateAddress(inputDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Create(inputDto));
        _addressServiceMock.Verify(service => service.CreateAddress(inputDto), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Update_ShouldReturnOkWithUpdatedAddressWhenIdsMatch()
    {
        // Arrange
        const int addressId = 1;
        var addressDto = AddressTestHelper.CreateValidAddressDTO(addressId);

        _addressServiceMock
            .Setup(service => service.UpdateAddress(addressDto))
            .ReturnsAsync(addressDto);

        // Act
        var result = await _controller.Update(addressId, addressDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedAddress = Assert.IsType<AddressDTO>(okResult.Value);
        Assert.Equal(addressId, returnedAddress.Id);

        _addressServiceMock.Verify(service => service.UpdateAddress(addressDto), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Update_ShouldReturnBadRequestWhenIdMismatch()
    {
        // Arrange
        const int routeId = 1;
        var addressDto = AddressTestHelper.CreateValidAddressDTO(2); // Mismatched ID

        // Act
        var result = await _controller.Update(routeId, addressDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("ID Mismatch", badRequestResult.Value);

        _addressServiceMock.Verify(service => service.UpdateAddress(It.IsAny<AddressDTO>()), Times.Never);
    }

    [Fact]
    public async Task AddressesController_Update_ShouldThrowKeyNotFoundExceptionWhenAddressDoesNotExistInService()
    {
        // Arrange
        const int addressId = 999;
        var addressDto = AddressTestHelper.CreateValidAddressDTO(addressId);

        _addressServiceMock
            .Setup(service => service.UpdateAddress(addressDto))
            .ThrowsAsync(new KeyNotFoundException($"Address with ID {addressId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Update(addressId, addressDto));
        _addressServiceMock.Verify(service => service.UpdateAddress(addressDto), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Update_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int addressId = 1;
        var addressDto = AddressTestHelper.CreateValidAddressDTO(addressId);

        _addressServiceMock
            .Setup(service => service.UpdateAddress(addressDto))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Update(addressId, addressDto));
        _addressServiceMock.Verify(service => service.UpdateAddress(addressDto), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Delete_ShouldReturnNoContentWhenDeleteIsSuccessful()
    {
        // Arrange
        const int addressId = 1;

        _addressServiceMock
            .Setup(service => service.DeleteAddress(addressId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(addressId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _addressServiceMock.Verify(service => service.DeleteAddress(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Delete_ShouldThrowKeyNotFoundExceptionWhenAddressDoesNotExist()
    {
        // Arrange
        const int addressId = 999;

        _addressServiceMock
            .Setup(service => service.DeleteAddress(addressId))
            .ThrowsAsync(new KeyNotFoundException($"Address with ID {addressId} not found."));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Delete(addressId));
        _addressServiceMock.Verify(service => service.DeleteAddress(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressesController_Delete_ShouldThrowExceptionWhenServiceFails()
    {
        // Arrange
        const int addressId = 1;

        _addressServiceMock
            .Setup(service => service.DeleteAddress(addressId))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Delete(addressId));
        _addressServiceMock.Verify(service => service.DeleteAddress(addressId), Times.Once);
    }
}