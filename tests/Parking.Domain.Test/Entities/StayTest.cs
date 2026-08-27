using FluentAssertions;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.Validations;

namespace Parking.Domain.Test.Entities;

public class StayTest
{
    [Fact(DisplayName = "Create Parameterless Stay Should Success")]
    public void CreateStay_ParameterlessConstructor_ResultObjectValidState()
    {
        // Arrange & Act
        var stay = new Stay();

        // Assert
        stay.Should().NotBeNull();
    }

    [Fact(DisplayName = "Create Stay With Valid State Should Success")]
    public void CreateStay_WithValidParameters_ResultObjectValidState()
    {
        // Arrange & Act
        Action action = () => new Stay(
            1,
            10,
            "ABC-1234",
            DateTime.UtcNow.AddHours(-1),
            null,
            10.00m,
            null,
            StayStatus.Parked,
            null
        );

        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create Stay Should Correctly Set All Properties")]
    public void CreateStay_ValidParameters_ShouldPopulatePropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        int customerVehicleId = 10;
        string licensePlate = "ABC-1234";
        DateTime entryDate = DateTime.UtcNow.AddHours(-2);
        DateTime exitDate = DateTime.UtcNow;
        decimal hourlyRate = 15.00m;
        decimal totalAmount = 30.00m;
        StayStatus status = StayStatus.Completed;

        // Act
        var stay = new Stay(id, customerVehicleId, licensePlate, entryDate, exitDate, hourlyRate, totalAmount, status, null);

        // Assert
        stay.Id.Should().Be(id);
        stay.CustomerVehicleId.Should().Be(customerVehicleId);
        stay.LicensePlate.Should().Be(licensePlate);
        stay.EntryDate.Should().Be(entryDate);
        stay.ExitDate.Should().Be(exitDate);
        stay.HourlyRate.Should().Be(hourlyRate);
        stay.TotalAmount.Should().Be(totalAmount);
        stay.StayStatus.Should().Be(status);
        stay.CustomerVehicle.Should().BeNull();
    }

    [Theory(DisplayName = "Create Stay With Invalid CustomerVehicleId Should Throw Exception")]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateStay_InvalidCustomerVehicleId_ShouldThrowDomainException(int? invalidCustomerVehicleId)
    {
        // Arrange & Act
        Action action = () => new Stay(1, invalidCustomerVehicleId, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*CustomerVehicleId is required and must be greater than zero*");
    }

    [Theory(DisplayName = "Create Stay With Invalid LicensePlate Should Throw Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateStay_InvalidLicensePlate_ShouldThrowDomainException(string invalidLicensePlate)
    {
        // Arrange & Act
        Action action = () => new Stay(1, 10, invalidLicensePlate, DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*LicensePlate is required*");
    }

    [Fact(DisplayName = "Create Stay With LicensePlate Exceeding 10 Characters Should Throw Exception")]
    public void CreateStay_LicensePlateExceeds10Characters_ShouldThrowDomainException()
    {
        // Arrange
        string longPlate = "ABCDEFGHIJK"; // 11 caracteres

        // Act
        Action action = () => new Stay(1, 10, longPlate, DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*LicensePlate cannot exceed 10 characters*");
    }

    [Fact(DisplayName = "Create Stay With Future EntryDate Should Throw Exception")]
    public void CreateStay_FutureEntryDate_ShouldThrowDomainException()
    {
        // Arrange
        DateTime futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        Action action = () => new Stay(1, 10, "ABC-1234", futureDate, null, 10.00m, null, StayStatus.Parked, null);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*EntryDate cannot be in the future*");
    }

    [Theory(DisplayName = "Create Stay With Invalid HourlyRate Should Throw Exception")]
    [InlineData(0)]
    [InlineData(-10.50)]
    public void CreateStay_InvalidHourlyRate_ShouldThrowDomainException(decimal invalidHourlyRate)
    {
        // Arrange & Act
        Action action = () => new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, invalidHourlyRate, null, StayStatus.Parked, null);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*HourlyRate must be greater than zero*");
    }

    [Fact(DisplayName = "UpdateStatus Should Update StayStatus Property")]
    public void UpdateStatus_ValidNewStatus_ShouldUpdateStayStatus()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        stay.UpdateStatus(StayStatus.Completed);

        // Assert
        stay.StayStatus.Should().Be(StayStatus.Completed);
    }

    [Fact(DisplayName = "UpdateStatus Should Update StayStatus To Cancelled Successfully")]
    public void UpdateStatus_ToCancelled_ShouldUpdateStayStatus()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        stay.UpdateStatus(StayStatus.Cancelled);

        // Assert
        stay.StayStatus.Should().Be(StayStatus.Cancelled);
    }

    [Fact(DisplayName = "UpdateStatus With Undefined Enum Value Should Assign Value Without Throwing Exception")]
    public void UpdateStatus_UndefinedEnumValue_ShouldAssignValue()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);
        var undefinedStatus = (StayStatus)99;

        // Act
        stay.UpdateStatus(undefinedStatus);

        // Assert
        stay.StayStatus.Should().Be(undefinedStatus);
    }

    [Fact(DisplayName = "CalculateStayHours Should Return Zero When EntryDate Or ExitDate Is Null")]
    public void CalculateStayHours_NullDates_ShouldReturnZero()
    {
        // Arrange
        var stayWithoutExitDate = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);
        var stayWithoutEntryDate = new Stay(1, 10, "ABC-1234", null, DateTime.UtcNow, 10.00m, null, StayStatus.Parked, null);

        // Act
        double hours1 = stayWithoutExitDate.CalculateStayHours();
        double hours2 = stayWithoutEntryDate.CalculateStayHours();

        // Assert
        hours1.Should().Be(0);
        hours2.Should().Be(0);
    }

    [Fact(DisplayName = "CalculateStayHours Should Return Correct Difference In Hours")]
    public void CalculateStayHours_ValidDates_ShouldReturnTotalHours()
    {
        // Arrange
        DateTime entry = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        DateTime exit = new DateTime(2026, 1, 1, 12, 30, 0, DateTimeKind.Utc); // 2.5 horas
        var stay = new Stay(1, 10, "ABC-1234", entry, exit, 10.00m, null, StayStatus.Parked, null);

        // Act
        double hours = stay.CalculateStayHours();

        // Assert
        hours.Should().Be(2.5);
    }

    [Fact(DisplayName = "CalculateTotalAmount Should Multiply Hours By HourlyRate")]
    public void CalculateTotalAmount_ValidHours_ShouldCalculateCorrectAmount()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 12.50m, null, StayStatus.Parked, null);

        // Act
        decimal total = stay.CalculateTotalAmount(2.0);

        // Assert
        total.Should().Be(25.00m);
    }

    [Fact(DisplayName = "CalculateTotalAmount With Zero Hours Should Return Zero")]
    public void CalculateTotalAmount_ZeroHours_ShouldReturnZero()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 12.50m, null, StayStatus.Parked, null);

        // Act
        decimal total = stay.CalculateTotalAmount(0);

        // Assert
        total.Should().Be(0m);
    }

    [Fact(DisplayName = "CalculateTotalAmount With Fractional Hours Should Calculate Correctly")]
    public void CalculateTotalAmount_FractionalHours_ShouldCalculateCorrectAmount()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        decimal total = stay.CalculateTotalAmount(1.5); // 1 hora e meia

        // Assert
        total.Should().Be(15.00m);
    }

    [Fact(DisplayName = "CalculateTotalAmount With Negative Hours Should Return Negative Value")]
    public void CalculateTotalAmount_NegativeHours_ShouldReturnNegativeAmount()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        decimal total = stay.CalculateTotalAmount(-1.0);

        // Assert
        total.Should().Be(-10.00m);
    }

    [Fact(DisplayName = "CompleteStay With Default ExitDate Should Success")]
    public void CompleteStay_DefaultExitDate_ShouldCompleteStayAndSetTotalAmount()
    {
        // Arrange
        DateTime entry = DateTime.UtcNow.AddHours(-2);
        var stay = new Stay(1, 10, "ABC-1234", entry, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        stay.CompleteStay();

        // Assert
        stay.StayStatus.Should().Be(StayStatus.Completed);
        stay.ExitDate.Should().NotBeNull();
        stay.TotalAmount.Should().BeGreaterThan(0);
    }

    [Fact(DisplayName = "CompleteStay With Custom ExitDate Should Success")]
    public void CompleteStay_CustomExitDate_ShouldCompleteStayCorrectly()
    {
        // Arrange
        DateTime entry = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        DateTime exit = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc); // 2 horas
        var stay = new Stay(1, 10, "ABC-1234", entry, null, 15.00m, null, StayStatus.Parked, null);

        // Act
        stay.CompleteStay(exit);

        // Assert
        stay.StayStatus.Should().Be(StayStatus.Completed);
        stay.ExitDate.Should().Be(exit);
        stay.TotalAmount.Should().Be(30.00m);
    }

    [Fact(DisplayName = "CompleteStay When Not Parked Should Throw Exception")]
    public void CompleteStay_NotParkedStatus_ShouldThrowDomainException()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow, 10.00m, 10.00m, StayStatus.Completed, null);

        // Act
        Action action = () => stay.CompleteStay();

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Only parked stays can be completed*");
    }

    [Fact(DisplayName = "CompleteStay With ExitDate Earlier Than EntryDate Should Throw Exception")]
    public void CompleteStay_ExitDateEarlierThanEntryDate_ShouldThrowDomainException()
    {
        // Arrange
        DateTime entry = DateTime.UtcNow;
        DateTime exit = entry.AddHours(-1);
        var stay = new Stay(1, 10, "ABC-1234", entry, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        Action action = () => stay.CompleteStay(exit);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*ExitDate cannot be earlier than EntryDate*");
    }

    [Fact(DisplayName = "CancelStay Within 5 Minutes Should Success")]
    public void CancelStay_Within5Minutes_ShouldCancelStay()
    {
        // Arrange
        DateTime entry = DateTime.UtcNow.AddMinutes(-3);
        DateTime cancelTime = entry.AddMinutes(4); // 4 minutos de diferença
        var stay = new Stay(1, 10, "ABC-1234", entry, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        stay.CancelStay(cancelTime);

        // Assert
        stay.StayStatus.Should().Be(StayStatus.Cancelled);
    }

    [Fact(DisplayName = "CancelStay With Default CurrentDate Should Success")]
    public void CancelStay_DefaultCurrentDate_ShouldCancelStay()
    {
        // Arrange
        DateTime entry = DateTime.UtcNow;
        var stay = new Stay(1, 10, "ABC-1234", entry, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        stay.CancelStay();

        // Assert
        stay.StayStatus.Should().Be(StayStatus.Cancelled);
    }

    [Fact(DisplayName = "CancelStay Without EntryDate Should Success")]
    public void CancelStay_NullEntryDate_ShouldCancelStay()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", null, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        stay.CancelStay();

        // Assert
        stay.StayStatus.Should().Be(StayStatus.Cancelled);
    }

    [Fact(DisplayName = "CancelStay When Not Parked Should Throw Exception")]
    public void CancelStay_NotParkedStatus_ShouldThrowDomainException()
    {
        // Arrange
        var stay = new Stay(1, 10, "ABC-1234", DateTime.UtcNow, null, 10.00m, null, StayStatus.Cancelled, null);

        // Act
        Action action = () => stay.CancelStay();

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Only parked stays can be cancelled*");
    }

    [Fact(DisplayName = "CancelStay Exceeding 5 Minutes Should Throw Exception")]
    public void CancelStay_Exceeds5Minutes_ShouldThrowDomainException()
    {
        // Arrange
        DateTime entry = DateTime.UtcNow.AddMinutes(-10);
        DateTime cancelTime = entry.AddMinutes(6); // 6 minutos de diferença
        var stay = new Stay(1, 10, "ABC-1234", entry, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        Action action = () => stay.CancelStay(cancelTime);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Stay can only be cancelled within 5 minutes of entry*");
    }

    [Fact(DisplayName = "CancelStay With Cancellation Date Earlier Than EntryDate Should Throw Exception")]
    public void CancelStay_CancellationDateEarlierThanEntryDate_ShouldThrowDomainException()
    {
        // Arrange
        DateTime entry = DateTime.UtcNow;
        DateTime cancelTime = entry.AddMinutes(-1);
        var stay = new Stay(1, 10, "ABC-1234", entry, null, 10.00m, null, StayStatus.Parked, null);

        // Act
        Action action = () => stay.CancelStay(cancelTime);

        // Assert
        action.Should().Throw<DomainExceptionValidation>()
              .WithMessage("*Cancellation date cannot be earlier than EntryDate*");
    }
}