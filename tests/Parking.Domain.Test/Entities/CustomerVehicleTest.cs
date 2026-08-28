using FluentAssertions;
using Parking.Domain.Entities;
using Parking.Domain.Validations;

namespace Parking.Domain.Test.Entities;

public class CustomerVehicleTest
{
    [Fact(DisplayName = "Create CustomerVehicle With Valid State Should Success")]
    public void CreateCustomerVehicle_WithValidParameters_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new CustomerVehicle(1, 10, 20);

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create CustomerVehicle Should Correctly Set All Properties")]
    public void CreateCustomerVehicle_ValidParameters_ShouldPopulatePropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        int customerId = 10;
        int vehicleId = 20;

        // Act
        var customerVehicle = new CustomerVehicle(id, customerId, vehicleId);

        // Assert
        customerVehicle.Id.Should().Be(id);
        customerVehicle.CustomerId.Should().Be(customerId);
        customerVehicle.VehicleId.Should().Be(vehicleId);
        customerVehicle.Stays.Should().NotBeNull().And.BeEmpty();
    }

    [Theory(DisplayName = "Create CustomerVehicle With Invalid CustomerId Should Throw Exception")]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateCustomerVehicle_InvalidCustomerId_ShouldThrowDomainException(int? invalidCustomerId)
    {
        // Arrange & Act
        Action action = () => new CustomerVehicle(1, invalidCustomerId, 20);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*CustomerId is required and must be greater than zero*");
    }

    [Theory(DisplayName = "Create CustomerVehicle With Invalid VehicleId Should Throw Exception")]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateCustomerVehicle_InvalidVehicleId_ShouldThrowDomainException(int? invalidVehicleId)
    {
        // Arrange & Act
        Action action = () => new CustomerVehicle(1, 10, invalidVehicleId);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*VehicleId is required and must be greater than zero*");
    }
}