using FluentAssertions;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.Validations;

namespace Parking.Domain.Test.Entities;

public class VehicleTest
{
    [Fact(DisplayName = "Create Vehicle With Valid State Should Success")]
    public void CreateVehicle_WithValidParameters_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new Vehicle(
            1,
            VehicleType.Car,
            "Toyota",
            "Corolla",
            "Black",
            2022,
            "No scratches"
        );

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create Vehicle With Null VehicleYear And Null Notes Should Success")]
    public void CreateVehicle_WithNullOptionalFields_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new Vehicle(
            1,
            VehicleType.Motorcycle,
            "Honda",
            "Civic",
            "Red",
            null,
            null
        );

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create Vehicle Should Correctly Set All Properties")]
    public void CreateVehicle_ValidParameters_ShouldPopulatePropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        VehicleType vehicleType = VehicleType.Van;
        string brand = "Ford";
        string model = "Transit";
        string color = "White";
        int? vehicleYear = 2021;
        string notes = "Company van";

        // Act
        var vehicle = new Vehicle(id, vehicleType, brand, model, color, vehicleYear, notes);

        // Assert
        vehicle.Id.Should().Be(id);
        vehicle.VehicleType.Should().Be(vehicleType);
        vehicle.Brand.Should().Be(brand);
        vehicle.Model.Should().Be(model);
        vehicle.Color.Should().Be(color);
        vehicle.VehicleYear.Should().Be(vehicleYear);
        vehicle.Notes.Should().Be(notes);
        vehicle.CustomerVehicles.Should().NotBeNull().And.BeEmpty();
    }

    [Theory(DisplayName = "Create Vehicle With Invalid Brand Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateVehicle_InvalidBrand_ShouldThrowDomainException(string invalidBrand)
    {
        // Arrange & Act
        Action action = () => new Vehicle(1, VehicleType.Car, invalidBrand, "Corolla", "Black", 2022, "Notes");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Brand is required*");
    }

    [Fact(DisplayName = "Create Vehicle With Brand Exceeding 50 Characters Should Throw Exception")]
    public void CreateVehicle_BrandExceeds50Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longBrand = new string('A', 51);

        // Act
        Action action = () => new Vehicle(1, VehicleType.Car, longBrand, "Corolla", "Black", 2022, "Notes");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Brand cannot exceed 50 characters*");
    }

    [Theory(DisplayName = "Create Vehicle With Invalid Model Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateVehicle_InvalidModel_ShouldThrowDomainException(string invalidModel)
    {
        // Arrange & Act
        Action action = () => new Vehicle(1, VehicleType.Car, "Toyota", invalidModel, "Black", 2022, "Notes");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Model is required*");
    }

    [Fact(DisplayName = "Create Vehicle With Model Exceeding 50 Characters Should Throw Exception")]
    public void CreateVehicle_ModelExceeds50Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longModel = new string('A', 51);

        // Act
        Action action = () => new Vehicle(1, VehicleType.Car, "Toyota", longModel, "Black", 2022, "Notes");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Model cannot exceed 50 characters*");
    }

    [Theory(DisplayName = "Create Vehicle With Invalid Color Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateVehicle_InvalidColor_ShouldThrowDomainException(string invalidColor)
    {
        // Arrange & Act
        Action action = () => new Vehicle(1, VehicleType.Car, "Toyota", "Corolla", invalidColor, 2022, "Notes");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Color is required*");
    }

    [Fact(DisplayName = "Create Vehicle With Color Exceeding 50 Characters Should Throw Exception")]
    public void CreateVehicle_ColorExceeds50Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longColor = new string('A', 51);

        // Act
        Action action = () => new Vehicle(1, VehicleType.Car, "Toyota", "Corolla", longColor, 2022, "Notes");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Color cannot exceed 50 characters*");
    }

    [Theory(DisplayName = "Create Vehicle With Invalid VehicleYear Range Should Throw Exception")]
    [InlineData(1899)]
    [InlineData(2101)]
    public void CreateVehicle_VehicleYearOutOfRange_ShouldThrowDomainException(int invalidYear)
    {
        // Arrange & Act
        Action action = () => new Vehicle(1, VehicleType.Car, "Toyota", "Corolla", "Black", invalidYear, "Notes");

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*VehicleYear must be between 1900 and 2100*");
    }

    [Fact(DisplayName = "Create Vehicle With Notes Exceeding 200 Characters Should Throw Exception")]
    public void CreateVehicle_NotesExceeds200Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longNotes = new string('A', 201);

        // Act
        Action action = () => new Vehicle(1, VehicleType.Car, "Toyota", "Corolla", "Black", 2022, longNotes);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Notes cannot exceed 200 characters*");
    }
}