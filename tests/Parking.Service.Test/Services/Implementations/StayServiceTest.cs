using MapsterMapper;
using Moq;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Implementations;
using Parking.Service.Test.Helpers;

namespace Parking.Service.Test.Services.Implementations;

public class StayServiceTest
{
    private readonly Mock<IStayRepository> _stayRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly StayService _stayService;

    public StayServiceTest()
    {
        _stayRepositoryMock = new Mock<IStayRepository>();
        _mapperMock = new Mock<IMapper>();
        _stayService = new StayService(_stayRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task StayService_GetAllStays_ShouldReturnAllStays()
    {
        // Arrange
        var stayList = new List<Stay> { StayTestHelper.CreateValidStayEntity(1) };
        var stayDtoList = new List<StayDTO> { StayTestHelper.CreateValidStayDTO(1) };

        _stayRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(stayList);

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<StayDTO>>(stayList))
            .Returns(stayDtoList);

        // Act
        var result = await _stayService.GetAllStays();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _stayRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<StayDTO>>(stayList), Times.Once);
    }

    [Fact]
    public async Task StayService_GetAllStays_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        _stayRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _stayService.GetAllStays());
        _stayRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task StayService_GetStayById_ShouldReturnStayById()
    {
        // Arrange
        const int stayId = 1;
        var stayEntity = StayTestHelper.CreateValidStayEntity(stayId);
        var stayDto = StayTestHelper.CreateValidStayDTO(stayId);

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync(stayEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<StayDTO>(stayEntity))
            .Returns(stayDto);

        // Act
        var result = await _stayService.GetStayById(stayId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stayId, result.Id);
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<StayDTO>(stayEntity), Times.Once);
    }

    [Fact]
    public async Task StayService_GetStayById_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        const int stayId = 999;

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync((Stay)null!);

        _mapperMock
            .Setup(mapper => mapper.Map<StayDTO>(It.IsAny<Stay>()))
            .Returns((StayDTO)null!);

        // Act
        var result = await _stayService.GetStayById(stayId);

        // Assert
        Assert.Null(result);
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
    }

    [Fact]
    public async Task StayService_GetStayById_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int stayId = 1;

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _stayService.GetStayById(stayId));
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
    }

    [Fact]
    public async Task StayService_CreateStay_ShouldCreateStay()
    {
        // Arrange
        var inputDto = StayTestHelper.CreateValidStayDTO(0);
        var mappedEntity = StayTestHelper.CreateValidStayEntity(0);
        var createdEntity = StayTestHelper.CreateValidStayEntity(1);
        var resultDto = StayTestHelper.CreateValidStayDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Stay>(inputDto))
            .Returns(mappedEntity);

        _stayRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ReturnsAsync(createdEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<StayDTO>(createdEntity))
            .Returns(resultDto);

        // Act
        var result = await _stayService.CreateStay(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<Stay>(inputDto), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<StayDTO>(createdEntity), Times.Once);
    }

    [Fact]
    public async Task StayService_CreateStay_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = StayTestHelper.CreateValidStayDTO(1);
        var mappedEntity = StayTestHelper.CreateValidStayEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Stay>(inputDto))
            .Returns(mappedEntity);

        _stayRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _stayService.CreateStay(inputDto));
        _stayRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
    }

    [Fact]
    public async Task StayService_CompleteStay_ShouldCompleteAndReturnStay()
    {
        // Arrange
        const int stayId = 1;
        var exitDate = DateTime.Now;
        var existingEntity = StayTestHelper.CreateValidStayEntity(stayId);
        var updatedEntity = StayTestHelper.CreateValidStayEntity(stayId);
        var resultDto = StayTestHelper.CreateValidStayDTO(stayId);

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync(existingEntity);

        _stayRepositoryMock
            .Setup(repo => repo.UpdateAsync(existingEntity))
            .ReturnsAsync(updatedEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<StayDTO>(updatedEntity))
            .Returns(resultDto);

        // Act
        var result = await _stayService.CompleteStay(stayId, exitDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stayId, result.Id);
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.UpdateAsync(existingEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<StayDTO>(updatedEntity), Times.Once);
    }

    [Fact]
    public async Task StayService_CompleteStay_ShouldThrowKeyNotFoundExceptionWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;
        var exitDate = DateTime.Now;

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync((Stay)null!);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _stayService.CompleteStay(stayId, exitDate));
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Stay>()), Times.Never);
    }

    [Fact]
    public async Task StayService_CompleteStay_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int stayId = 1;
        var exitDate = DateTime.Now;
        var existingEntity = StayTestHelper.CreateValidStayEntity(stayId);

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync(existingEntity);

        _stayRepositoryMock
            .Setup(repo => repo.UpdateAsync(existingEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _stayService.CompleteStay(stayId, exitDate));
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.UpdateAsync(existingEntity), Times.Once);
    }

    [Fact]
    public async Task StayService_CancelStay_ShouldCancelAndReturnStay()
    {
        // Arrange
        const int stayId = 1;
        var now = DateTime.UtcNow;

        var existingEntity = new Stay(stayId, 1, "ABC1234", now, null, 10.0m, null, StayStatus.Parked, null!);
        var updatedEntity = new Stay(stayId, 1, "ABC1234", now, null, 10.0m, null, StayStatus.Cancelled, null!);
        var resultDto = StayTestHelper.CreateValidStayDTO(stayId);

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync(existingEntity);

        _stayRepositoryMock
            .Setup(repo => repo.UpdateAsync(existingEntity))
            .ReturnsAsync(updatedEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<StayDTO>(updatedEntity))
            .Returns(resultDto);

        // Act
        var result = await _stayService.CancelStay(stayId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stayId, result.Id);
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.UpdateAsync(existingEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<StayDTO>(updatedEntity), Times.Once);
    }

    [Fact]
    public async Task StayService_CancelStay_ShouldThrowKeyNotFoundExceptionWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync((Stay)null!);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _stayService.CancelStay(stayId));
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Stay>()), Times.Never);
    }

    [Fact]
    public async Task StayService_CancelStay_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int stayId = 1;
        var now = DateTime.UtcNow;
        var existingEntity = new Stay(stayId, 1, "ABC1234", now, null, 10.0m, null, StayStatus.Parked, null!);

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync(existingEntity);

        _stayRepositoryMock
            .Setup(repo => repo.UpdateAsync(existingEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _stayService.CancelStay(stayId));
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.UpdateAsync(existingEntity), Times.Once);
    }

    [Fact]
    public async Task StayService_DeleteStay_ShouldDeleteStay()
    {
        // Arrange
        const int stayId = 1;
        var existingEntity = StayTestHelper.CreateValidStayEntity(stayId);

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync(existingEntity);

        _stayRepositoryMock
            .Setup(repo => repo.DeleteAsync(stayId))
            .Returns(Task.CompletedTask);

        // Act
        await _stayService.DeleteStay(stayId);

        // Assert
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.DeleteAsync(stayId), Times.Once);
    }

    [Fact]
    public async Task StayService_DeleteStay_ShouldThrowKeyNotFoundExceptionWhenStayDoesNotExist()
    {
        // Arrange
        const int stayId = 999;

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync((Stay)null!);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _stayService.DeleteStay(stayId));
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task StayService_DeleteStay_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int stayId = 1;
        var existingEntity = StayTestHelper.CreateValidStayEntity(stayId);

        _stayRepositoryMock
            .Setup(repo => repo.GetByIdAsync(stayId))
            .ReturnsAsync(existingEntity);

        _stayRepositoryMock
            .Setup(repo => repo.DeleteAsync(stayId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _stayService.DeleteStay(stayId));
        _stayRepositoryMock.Verify(repo => repo.GetByIdAsync(stayId), Times.Once);
        _stayRepositoryMock.Verify(repo => repo.DeleteAsync(stayId), Times.Once);
    }
}