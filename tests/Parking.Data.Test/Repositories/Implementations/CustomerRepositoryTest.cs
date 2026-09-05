using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Parking.Data.Context;
using Parking.Data.Implementations;
using Parking.Data.Test.Context.Helpers;
using Parking.Domain.Entities;

namespace Parking.Data.Test.Repositories.Implementations;

public class CustomerRepositoryTest
{
    private readonly Mock<ILogger<Customer>> _loggerMock = new();

    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return All Customers")]
    public async Task CustomerRepository_GetAllAsync_ShouldReturnAllCustomers()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Customers.AddRange(GetSampleCustomers());
        await context.SaveChangesAsync();

        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact(DisplayName = "GetAllAsync - Positive: Should Return Empty List When No Customers Exist")]
    public async Task CustomerRepository_GetAllAsync_ShouldReturnEmptyListWhenNoCustomersExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "GetAllAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task CustomerRepository_GetAllAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllAsync());
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Customer When Id Exists")]
    public async Task CustomerRepository_GetByIdAsync_ShouldReturnCustomerWhenIdExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Customers.AddRange(GetSampleCustomers());
        await context.SaveChangesAsync();

        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact(DisplayName = "GetByIdAsync - Positive: Should Return Null When Id Does Not Exist")]
    public async Task CustomerRepository_GetByIdAsync_ShouldReturnNullWhenIdDoesNotExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetByIdAsync - Negative: Should Throw Exception And Log Error When Database Fails")]
    public async Task CustomerRepository_GetByIdAsync_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetByIdAsync(1));
    }

    [Fact(DisplayName = "AddAsync - Positive: Should Return Added Customer")]
    public async Task CustomerRepository_AddAsync_ShouldReturnAddedCustomer()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);
        var newCustomer = new Customer(
            4,
            "David Miller",
            new DateOnly(1995, 4, 25),
            "44455566677",
            "5511944445555",
            "david.miller@email.com",
            4
        );

        // Act
        var result = await repository.AddAsync(newCustomer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("David Miller", result.Name);
        Assert.Equal(1, context.Customers.Count());
    }

    [Fact(DisplayName = "AddAsync - Negative: Should Throw Exception And Log Error When Insert Fails")]
    public async Task CustomerRepository_AddAsync_ShouldThrowExceptionWhenInsertFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);
        var newCustomer = new Customer(
            4,
            "David Miller",
            new DateOnly(1995, 4, 25),
            "44455566677",
            "5511944445555",
            "david.miller@email.com",
            4
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(newCustomer));
    }

    [Fact(DisplayName = "UpdateAsync - Positive: Should Return Updated Customer")]
    public async Task CustomerRepository_UpdateAsync_ShouldReturnUpdatedCustomer()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var customer = new Customer(
            1,
            "John Doe",
            new DateOnly(1990, 1, 15),
            "11122233344",
            "5511911112222",
            "john.doe@email.com",
            1
        );
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        context.Entry(customer).State = EntityState.Detached;

        var repository = new CustomerRepository(context, _loggerMock.Object);
        var updatedCustomer = new Customer(
            1,
            "John Doe Updated",
            new DateOnly(1990, 1, 15),
            "11122233344",
            "5511911112222",
            "john.doe.updated@email.com",
            1
        );

        // Act
        var result = await repository.UpdateAsync(updatedCustomer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe Updated", result.Name);
        Assert.Equal("john.doe.updated@email.com", result.Email);
    }

    [Fact(DisplayName = "UpdateAsync - Negative: Should Throw Exception And Log Error When Update Fails")]
    public async Task CustomerRepository_UpdateAsync_ShouldThrowExceptionWhenUpdateFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);
        var updatedCustomer = new Customer(
            1,
            "John Doe Updated",
            new DateOnly(1990, 1, 15),
            "11122233344",
            "5511911112222",
            "john.doe.updated@email.com",
            1
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(updatedCustomer));
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Complete Successfully When Customer Exists")]
    public async Task CustomerRepository_DeleteAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var customer = new Customer(
            1,
            "John Doe",
            new DateOnly(1990, 1, 15),
            "11122233344",
            "5511911112222",
            "john.doe@email.com",
            1
        );
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        Assert.Equal(0, context.Customers.Count());
    }

    [Fact(DisplayName = "DeleteAsync - Positive: Should Handle Non Existing Customer Gracefully")]
    public async Task CustomerRepository_DeleteAsync_ShouldHandleNonExistingCustomerGracefully()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(99));

        // Assert
        Assert.Null(exception);
    }

    [Fact(DisplayName = "DeleteAsync - Negative: Should Throw Exception And Log Error When Delete Fails")]
    public async Task CustomerRepository_DeleteAsync_ShouldThrowExceptionWhenDeleteFails()
    {
        // Arrange
        using var context = new FaultyDbContext();
        var repository = new CustomerRepository(context, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DeleteAsync(1));
    }

    private List<Customer> GetSampleCustomers() => new()
    {
        new Customer(1, "John Doe", new DateOnly(1990, 1, 15), "11122233344", "5511911112222", "john.doe@email.com", 1),
        new Customer(2, "Jane Smith", new DateOnly(1985, 5, 20), "22233344455", "5511922223333", "jane.smith@email.com", 2),
        new Customer(3, "Bob Johnson", null, "33344455566", "5511933334444", "bob.johnson@email.com", 3)
    };
}