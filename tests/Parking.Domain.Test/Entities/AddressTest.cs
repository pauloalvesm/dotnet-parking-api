using FluentAssertions;
using Parking.Domain.Entities;
using Parking.Domain.Validations;

namespace Parking.Domain.Test.Entities;

public class AddressTest
{
    [Fact(DisplayName = "Create Address With Valid State And Complement Should Success")]
    public void CreateAddress_WithValidParametersAndComplement_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new Address(
            1,
            "Main Street",
            "123",
            "Apt 101",
            "Downtown",
            "NY",
            "New York",
            "10001-000"
        );

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create Address With Null Complement Should Success")]
    public void CreateAddress_WithNullComplement_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new Address(
            1,
            "Main Street",
            "123",
            null,
            "Downtown",
            "NY",
            "New York",
            "10001-000"
        );

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create Address Should Correctly Set All Properties")]
    public void CreateAddress_ValidParameters_ShouldPopulatePropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        string street = "Main Street";
        string number = "123";
        string complement = "Apt 101";
        string neighborhood = "Downtown";
        string federativeUnit = "NY";
        string city = "New York";
        string zipCode = "10001-000";

        // Act
        var address = new Address(id, street, number, complement, neighborhood, federativeUnit, city, zipCode);

        // Assert
        address.Id.Should().Be(id);
        address.Street.Should().Be(street);
        address.Number.Should().Be(number);
        address.Complement.Should().Be(complement);
        address.Neighborhood.Should().Be(neighborhood);
        address.FederativeUnit.Should().Be(federativeUnit);
        address.City.Should().Be(city);
        address.ZipCode.Should().Be(zipCode);
        address.Customers.Should().NotBeNull().And.BeEmpty();
    }

    [Theory(DisplayName = "Create Address With Invalid Street Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAddress_InvalidStreet_ShouldThrowDomainException(string invalidStreet)
    {
        // Arrange & Act
        Action action = () => new Address(1, invalidStreet, "123", "Apt 101", "Downtown", "NY", "New York", "10001-000");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Street is required*");
    }

    [Theory(DisplayName = "Create Address With Invalid Number Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAddress_InvalidNumber_ShouldThrowDomainException(string invalidNumber)
    {
        // Arrange & Act
        Action action = () => new Address(1, "Main Street", invalidNumber, "Apt 101", "Downtown", "NY", "New York", "10001-000");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Number is required*");
    }

    [Theory(DisplayName = "Create Address With Invalid Neighborhood Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAddress_InvalidNeighborhood_ShouldThrowDomainException(string invalidNeighborhood)
    {
        // Arrange & Act
        Action action = () => new Address(1, "Main Street", "123", "Apt 101", invalidNeighborhood, "NY", "New York", "10001-000");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Neighborhood is required*");
    }

    [Theory(DisplayName = "Create Address With Empty FederativeUnit Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAddress_InvalidFederativeUnit_ShouldThrowDomainException(string invalidFederativeUnit)
    {
        // Arrange & Act
        Action action = () => new Address(1, "Main Street", "123", "Apt 101", "Downtown", invalidFederativeUnit, "New York", "10001-000");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*FederativeUnit is required*");
    }

    [Theory(DisplayName = "Create Address With FederativeUnit Length Not Equal To 2 Should Throw Exception")]
    [InlineData("N")]
    [InlineData("NYY")]
    public void CreateAddress_FederativeUnitLengthNotTwo_ShouldThrowDomainException(string invalidFederativeUnit)
    {
        // Arrange & Act
        Action action = () => new Address(1, "Main Street", "123", "Apt 101", "Downtown", invalidFederativeUnit, "New York", "10001-000");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*FederativeUnit must be 2 characters long*");
    }

    [Theory(DisplayName = "Create Address With Invalid City Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAddress_InvalidCity_ShouldThrowDomainException(string invalidCity)
    {
        // Arrange & Act
        Action action = () => new Address(1, "Main Street", "123", "Apt 101", "Downtown", "NY", invalidCity, "10001-000");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*City is required*");
    }

    [Theory(DisplayName = "Create Address With Empty ZipCode Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAddress_InvalidZipCode_ShouldThrowDomainException(string invalidZipCode)
    {
        // Arrange & Act
        Action action = () => new Address(1, "Main Street", "123", "Apt 101", "Downtown", "NY", "New York", invalidZipCode);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*ZipCode is required*");
    }

    [Theory(DisplayName = "Create Address With ZipCode Length Not Equal To 9 Should Throw Exception")]
    [InlineData("10001000")]
    [InlineData("10001-0000")]
    public void CreateAddress_ZipCodeLengthNotNine_ShouldThrowDomainException(string invalidZipCode)
    {
        // Arrange & Act
        Action action = () => new Address(1, "Main Street", "123", "Apt 101", "Downtown", "NY", "New York", invalidZipCode);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*ZipCode must be 9 characters long*");
    }

    [Fact(DisplayName = "Create Address With Complement Longer Than 150 Characters Should Throw Exception")]
    public void CreateAddress_ComplementExceeds150Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longComplement = new string('A', 151);

        // Act
        Action action = () => new Address(1, "Main Street", "123", longComplement, "Downtown", "NY", "New York", "10001-000");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Complement cannot exceed 150 characters*");
    }
}