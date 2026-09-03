using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Parking.Data.Context;
using Parking.Data.Implementations;
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

    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private ApplicationDbContext GetFaultyDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=invalid_host;Database=invalid_db;Username=invalid;Password=invalid;Timeout=1")
            .Options;

        return new ApplicationDbContext(options);
    }

    private Stay CreateValidStay(int id = 1)
    {
        var customerVehicle = (CustomerVehicle)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(CustomerVehicle));
        var customer = (Customer)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Customer));
        var vehicle = (Vehicle)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Vehicle));

        // Preenche todas as propriedades de texto não inicializadas
        PopulateRequiredStringProperties(customer);
        PopulateRequiredStringProperties(vehicle);

        // Preenche campos numéricos e IDs obrigatórios para passar na validação do construtor
        SetPropertyIfExists(customer, "Id", 1);
        SetPropertyIfExists(customer, "AddressId", 1);
        SetPropertyIfExists(customer, "Cpf", "12345678901");
        SetPropertyIfExists(customer, "Phone", "11999999999");
        SetPropertyIfExists(customer, "Email", "test@test.com");

        SetPropertyIfExists(vehicle, "Id", 1);

        typeof(CustomerVehicle).GetProperty(nameof(CustomerVehicle.Customer))?.SetValue(customerVehicle, customer);
        typeof(CustomerVehicle).GetProperty(nameof(CustomerVehicle.Vehicle))?.SetValue(customerVehicle, vehicle);

        return new Stay(
            id: id,
            customerVehicleId: 1,
            licensePlate: "ABC-1234",
            entryDate: DateTime.UtcNow.AddHours(-2),
            exitDate: null,
            hourlyRate: 10.0m,
            totalAmount: null,
            stayStatus: StayStatus.Parked,
            customerVehicle: customerVehicle
        );
    }

    private static void PopulateRequiredStringProperties(object obj)
    {
        var properties = obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        foreach (var prop in properties)
        {
            if (prop.PropertyType == typeof(string) && prop.CanWrite && prop.GetValue(obj) == null)
            {
                prop.SetValue(obj, "TestValue");
            }
        }
    }

    private static void SetPropertyIfExists(object obj, string propertyName, object value)
    {
        var prop = obj.GetType().GetProperty(propertyName);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(obj, value);
        }
    }

    #region GetAllAsync Tests

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return All Stays With Navigation Properties")]
    public async Task GetAllAsync_Positive_ShouldReturnAllStays()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var stay = CreateValidStay(1);
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
        using var context = GetFaultyDbContext();
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

    #endregion

    #region GetByIdAsync Tests

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Stay By Id With Navigation Properties")]
    public async Task GetByIdAsync_Positive_ShouldReturnStayById()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var stay = CreateValidStay(1);
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
        using var context = GetInMemoryDbContext();
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
        using var context = GetFaultyDbContext();
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

    #endregion

    #region AddAsync Tests (Inherited from Repository<TEntity>)

    [Fact(DisplayName = "AddAsync - Positive: Should Add New Stay")]
    public async Task AddAsync_Positive_ShouldAddNewStay()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);
        var stay = CreateValidStay(1);

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
        using var context = GetFaultyDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);
        var stay = CreateValidStay(1);

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

    #endregion

    #region UpdateAsync Tests

    [Fact(DisplayName = "UpdateAsync - Positive: Should Update Existing Stay Status")]
    public async Task UpdateAsync_Positive_ShouldUpdateStay()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var stay = CreateValidStay(1);
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
        using var context = GetFaultyDbContext();
        var repository = new StayRepository(context, _loggerMock.Object);
        var stay = CreateValidStay(1);

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

    #endregion

    #region DeleteAsync Tests (Inherited from Repository<TEntity>)

    [Fact(DisplayName = "DeleteAsync - Positive: Should Delete Stay When Exists")]
    public async Task DeleteAsync_Positive_ShouldDeleteStay()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var stay = CreateValidStay(1);
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
        using var context = GetInMemoryDbContext();
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
        using var context = GetFaultyDbContext();
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

    #endregion
}