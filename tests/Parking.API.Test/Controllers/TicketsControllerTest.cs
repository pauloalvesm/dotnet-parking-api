using Microsoft.AspNetCore.Mvc;
using Moq;
using Parking.Api.Controllers;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;
using Parking.Test.Shared.Helpers;

namespace Parking.API.Test.Controllers;

public class TicketsControllerTest
{
    private readonly Mock<IStayService> _stayServiceMock;
    private readonly Mock<IPdfService> _pdfServiceMock;
    private readonly TicketsController _controller;

    public TicketsControllerTest()
    {
        _stayServiceMock = new Mock<IStayService>();
        _pdfServiceMock = new Mock<IPdfService>();
        _controller = new TicketsController(_stayServiceMock.Object, _pdfServiceMock.Object);
    }

    [Fact]
    public async Task TicketsController_GetTicket_ShouldReturnFileResultWhenStayExists()
    {
        // Arrange
        const int stayId = 1;
        var stayDto = StayTestHelper.CreateValidStayDTO(stayId);
        var expectedBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };

        _stayServiceMock
            .Setup(service => service.GetStayById(stayId))
            .ReturnsAsync(stayDto);

        _pdfServiceMock
            .Setup(service => service.GenerateStayPdf(stayDto))
            .Returns(expectedBytes);

        // Act
        var result = await _controller.GetTicket(stayId);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/pdf", fileResult.ContentType);
        Assert.Equal($"Stay_{stayId}.pdf", fileResult.FileDownloadName);
        Assert.Equal(expectedBytes, fileResult.FileContents);

        _stayServiceMock.Verify(service => service.GetStayById(stayId), Times.Once);
        _pdfServiceMock.Verify(service => service.GenerateStayPdf(stayDto), Times.Once);
    }

    [Fact]
    public async Task TicketsController_GetTicket_ShouldReturnNotFoundWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;

        _stayServiceMock
            .Setup(service => service.GetStayById(stayId))
            .ReturnsAsync((StayDTO)null!);

        // Act
        var result = await _controller.GetTicket(stayId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Stay with ID {stayId} not found.", notFoundResult.Value);

        _stayServiceMock.Verify(service => service.GetStayById(stayId), Times.Once);
        _pdfServiceMock.Verify(service => service.GenerateStayPdf(It.IsAny<StayDTO>()), Times.Never);
    }
}