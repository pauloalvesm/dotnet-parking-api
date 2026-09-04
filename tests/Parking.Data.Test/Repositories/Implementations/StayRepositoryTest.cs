using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Parking.Data.Implementations;
using Parking.Data.Test.Helpers;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Xunit;

namespace Parking.Data.Test.Repositories.Implementations;

public class StayRepositoryTest
{
    private readonly Mock<ILogger<Stay>> _loggerMock;

    public StayRepositoryTest()
    {
        _loggerMock = new Mock<ILogger<Stay>>();
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return All Stays With Navigation Properties")]
    public async Task GetAllAsync_Positive_ShouldReturnAllStays()
    {
        // Arrange
        using var context = StayTestHelper.GetInMemoryDbContext();
        var stay = StayTestHelper.CreateValidStay(1);
        context.Stays.Add(stay);
        await context.SaveChangesAsync();

        var repository = new StayRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var retrievedStay = result.First();
        Assert.Equal(stay.Id, retrievedStay.Id);
        Assert.NotNull(retrievedStay.CustomerVehicle);
        Assert.NotNull(retrievedStay.CustomerVehicle.Customer);
        Assert.NotNull(retrievedStay.CustomerVehicle.Vehicle);
    }

    [Fact(DisplayName = "GetAllAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task GetAllAsync_Negative_ShouldThrowExceptionAndLogError()
    {
        // Arrange
        using var context = StayTestHelper.GetFaultyDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllAsync());

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error when searching list of records")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Stay By Id With Navigation Properties")]
    public async Task GetByIdAsync_Positive_ShouldReturnStayById()
    {
        // Arrange
        using var context = StayTestHelper.GetInMemoryDbContext();
        var stay = StayTestHelper.CreateValidStay(1);
        context.Stays.Add(stay);
        await context.SaveChangesAsync();

        var repository = new StayRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.NotNull(result.CustomerVehicle);
        Assert.NotNull(result.CustomerVehicle.Customer);
        Assert.NotNull(result.CustomerVehicle.Vehicle);
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Null When Stay Id Does Not Exist")]
    public async Task GetByIdAsync_Positive_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        using var context = StayTestHelper.GetInMemoryDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetByIdAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task GetByIdAsync_Negative_ShouldThrowExceptionAndLogError()
    {
        // Arrange
        using var context = StayTestHelper.GetFaultyDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetByIdAsync(1));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error getting record with ID: 1")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact(DisplayName = "AddAsync - Positive: Should Add New Stay")]
    public async Task AddAsync_Positive_ShouldAddNewStay()
    {
        // Arrange
        using var context = StayTestHelper.GetInMemoryDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);
        var stay = StayTestHelper.CreateValidStay(1);

        // Act
        var result = await repository.AddAsync(stay);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, await context.Stays.CountAsync());
    }

    [Fact(DisplayName = "AddAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task AddAsync_Negative_ShouldThrowExceptionAndLogError()
    {
        // Arrange
        using var context = StayTestHelper.GetFaultyDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);
        var stay = StayTestHelper.CreateValidStay(1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(stay));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error when adding a new record")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact(DisplayName = "UpdateAsync - Positive: Should Update Existing Stay Status")]
    public async Task UpdateAsync_Positive_ShouldUpdateStay()
    {
        // Arrange
        using var context = StayTestHelper.GetInMemoryDbContext();
        var stay = StayTestHelper.CreateValidStay(1);
        context.Stays.Add(stay);
        await context.SaveChangesAsync();

        var repository = new StayRepository(context, _loggerMock.Object);

        // Act
        stay.CompleteStay(DateTime.UtcNow);
        var result = await repository.UpdateAsync(stay);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StayStatus.Completed, result.StayStatus);

        var updatedStayInDb = await context.Stays.FindAsync(1);
        Assert.Equal(StayStatus.Completed, updatedStayInDb.StayStatus);
    }

    [Fact(DisplayName = "UpdateAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task UpdateAsync_Negative_ShouldThrowExceptionAndLogError()
    {
        // Arrange
        using var context = StayTestHelper.GetFaultyDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);
        var stay = StayTestHelper.CreateValidStay(1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(stay));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error when updating the record")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Delete Stay When Exists")]
    public async Task DeleteAsync_Positive_ShouldDeleteStay()
    {
        // Arrange
        using var context = StayTestHelper.GetInMemoryDbContext();
        var stay = StayTestHelper.CreateValidStay(1);
        context.Stays.Add(stay);
        await context.SaveChangesAsync();

        var repository = new StayRepository(context, _loggerMock.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        Assert.Equal(0, await context.Stays.CountAsync());
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Do Nothing When Entity Does Not Exist")]
    public async Task DeleteAsync_Positive_ShouldDoNothingWhenEntityNotFound()
    {
        // Arrange
        using var context = StayTestHelper.GetInMemoryDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);

        // Act
        await repository.DeleteAsync(999);

        // Assert
        Assert.Equal(0, await context.Stays.CountAsync());
    }

    [Fact(DisplayName = "DeleteAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task DeleteAsync_Negative_ShouldThrowExceptionAndLogError()
    {
        // Arrange
        using var context = StayTestHelper.GetFaultyDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DeleteAsync(1));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error when deleting the record")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }

}