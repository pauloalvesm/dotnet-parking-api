using FluentAssertions;
using Parking.Domain.Entities;
using Parking.Domain.Validations;

namespace Parking.Domain.Test.Entities;

public class CustomerTest
{
    [Fact(DisplayName = "Create Customer With Valid State Should Success")]
    public void CreateCustomer_WithValidParameters_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new Customer(
            1,
            "John Doe",
            new DateOnly(1990, 5, 15),
            "12345678901",
            "+15551234567",
            "john.doe@email.com",
            10
        );

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create Customer With Null BirthDate Should Success")]
    public void CreateCustomer_WithNullBirthDate_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new Customer(
            1,
            "John Doe",
            null,
            "12345678901",
            "+15551234567",
            "john.doe@email.com",
            10
        );

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create Customer Should Correctly Set All Properties")]
    public void CreateCustomer_ValidParameters_ShouldPopulatePropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        string name = "John Doe";
        DateOnly? birthDate = new DateOnly(1990, 5, 15);
        string cpf = "12345678901";
        string phone = "+15551234567";
        string email = "john.doe@email.com";
        int addressId = 10;

        // Act
        var customer = new Customer(id, name, birthDate, cpf, phone, email, addressId);

        // Assert
        customer.Id.Should().Be(id);
        customer.Name.Should().Be(name);
        customer.BirthDate.Should().Be(birthDate);
        customer.Cpf.Should().Be(cpf);
        customer.Phone.Should().Be(phone);
        customer.Email.Should().Be(email);
        customer.AddressId.Should().Be(addressId);
        customer.CustomerVehicles.Should().NotBeNull().And.BeEmpty();
    }

    [Theory(DisplayName = "Create Customer With Invalid Name Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateCustomer_InvalidName_ShouldThrowDomainException(string invalidName)
    {
        // Arrange & Act
        Action action = () => new Customer(1, invalidName, null, "12345678901", "+15551234567", "john.doe@email.com", 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Name is required*");
    }

    [Fact(DisplayName = "Create Customer With Name Exceeding 100 Characters Should Throw Exception")]
    public void CreateCustomer_NameExceeds100Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longName = new string('A', 101);

        // Act
        Action action = () => new Customer(1, longName, null, "12345678901", "+15551234567", "john.doe@email.com", 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Name cannot exceed 100 characters*");
    }

    [Theory(DisplayName = "Create Customer With Invalid CPF Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateCustomer_InvalidCpf_ShouldThrowDomainException(string invalidCpf)
    {
        // Arrange & Act
        Action action = () => new Customer(1, "John Doe", null, invalidCpf, "+15551234567", "john.doe@email.com", 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*CPF is required*");
    }

    [Theory(DisplayName = "Create Customer With CPF Length Not Equal To 11 Should Throw Exception")]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    public void CreateCustomer_CpfLengthNotEleven_ShouldThrowDomainException(string invalidCpf)
    {
        // Arrange & Act
        Action action = () => new Customer(1, "John Doe", null, invalidCpf, "+15551234567", "john.doe@email.com", 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*CPF must be 11 characters long*");
    }

    [Theory(DisplayName = "Create Customer With Invalid Phone Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateCustomer_InvalidPhone_ShouldThrowDomainException(string invalidPhone)
    {
        // Arrange & Act
        Action action = () => new Customer(1, "John Doe", null, "12345678901", invalidPhone, "john.doe@email.com", 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Phone is required*");
    }

    [Fact(DisplayName = "Create Customer With Phone Exceeding 15 Characters Should Throw Exception")]
    public void CreateCustomer_PhoneExceeds15Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longPhone = new string('1', 16);

        // Act
        Action action = () => new Customer(1, "John Doe", null, "12345678901", longPhone, "john.doe@email.com", 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Phone cannot exceed 15 characters*");
    }

    [Theory(DisplayName = "Create Customer With Invalid Email Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateCustomer_InvalidEmail_ShouldThrowDomainException(string invalidEmail)
    {
        // Arrange & Act
        Action action = () => new Customer(1, "John Doe", null, "12345678901", "+15551234567", invalidEmail, 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Email is required*");
    }

    [Fact(DisplayName = "Create Customer With Email Exceeding 100 Characters Should Throw Exception")]
    public void CreateCustomer_EmailExceeds100Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longEmail = $"{new string('a', 92)}@email.com"; // 101 caracteres

        // Act
        Action action = () => new Customer(1, "John Doe", null, "12345678901", "+15551234567", longEmail, 10);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Email cannot exceed 100 characters*");
    }

    [Theory(DisplayName = "Create Customer With Invalid AddressId Should Throw Exception")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateCustomer_InvalidAddressId_ShouldThrowDomainException(int invalidAddressId)
    {
        // Arrange & Act
        Action action = () => new Customer(1, "John Doe", null, "12345678901", "+15551234567", "john.doe@email.com", invalidAddressId);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*AddressId is required and must be greater than zero*");
    }
}