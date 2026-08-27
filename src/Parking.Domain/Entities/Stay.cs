using Parking.Domain.Common;
using Parking.Domain.Enums;
using Parking.Domain.Validations;

namespace Parking.Domain.Entities;

public class Stay : Entity
{
    public int? CustomerVehicleId { get; private set; }
    public string LicensePlate { get; private set; }
    public DateTime? EntryDate { get; private set; }
    public DateTime? ExitDate { get; private set; }
    public decimal HourlyRate { get; private set; }
    public decimal? TotalAmount { get; private set; }
    public StayStatus StayStatus { get; private set; }

    public CustomerVehicle CustomerVehicle { get; private set; }

    public Stay() { }

    public Stay(int id,
                int? customerVehicleId,
                string licensePlate,
                DateTime? entryDate,
                DateTime? exitDate,
                decimal hourlyRate,
                decimal? totalAmount,
                StayStatus stayStatus,
                CustomerVehicle customerVehicle)
    {
        ValidateDomain(customerVehicleId, licensePlate, entryDate, hourlyRate);

        Id = id;
        CustomerVehicleId = customerVehicleId;
        LicensePlate = licensePlate;
        EntryDate = entryDate;
        ExitDate = exitDate;
        HourlyRate = hourlyRate;
        TotalAmount = totalAmount;
        StayStatus = stayStatus;
        CustomerVehicle = customerVehicle;
    }

    public void UpdateStatus(StayStatus newStatus)
    {
        StayStatus = newStatus;
    }

    public double CalculateStayHours()
    {
        if (EntryDate == null || ExitDate == null)
        {
            return 0;
        }

        TimeSpan hoursDifference = ExitDate.Value - EntryDate.Value;
        return hoursDifference.TotalHours;
    }

    public decimal CalculateTotalAmount(double hours)
    {
        decimal totalAmount = (decimal)hours * HourlyRate;
        return totalAmount;
    }

    public void CompleteStay(DateTime? exitDate = null)
    {
        DomainExceptionValidation.GetErrors(StayStatus != StayStatus.Parked, "Only parked stays can be completed");

        ExitDate = exitDate ?? DateTime.UtcNow;

        DomainExceptionValidation.GetErrors(EntryDate.HasValue && ExitDate < EntryDate, "ExitDate cannot be earlier than EntryDate");

        double hours = CalculateStayHours();
        TotalAmount = CalculateTotalAmount(hours);
        UpdateStatus(StayStatus.Completed);
    }

    public void CancelStay(DateTime? currentDate = null)
    {
        DomainExceptionValidation.GetErrors(StayStatus != StayStatus.Parked, "Only parked stays can be cancelled");

        if (EntryDate.HasValue)
        {
            DateTime cancelTime = currentDate ?? DateTime.UtcNow;
            TimeSpan timeElapsed = cancelTime - EntryDate.Value;

            DomainExceptionValidation.GetErrors(timeElapsed.TotalMinutes > 5, "Stay can only be cancelled within 5 minutes of entry");
            DomainExceptionValidation.GetErrors(cancelTime < EntryDate.Value, "Cancellation date cannot be earlier than EntryDate");
        }

        UpdateStatus(StayStatus.Cancelled);
    }

    private void ValidateDomain(int? customerVehicleId, string licensePlate, DateTime? entryDate, decimal hourlyRate)
    {
        DomainExceptionValidation.GetErrors(!customerVehicleId.HasValue || customerVehicleId <= 0, "CustomerVehicleId is required and must be greater than zero");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(licensePlate), "LicensePlate is required");
        DomainExceptionValidation.GetErrors(licensePlate.Length > 10, "LicensePlate cannot exceed 10 characters");
        DomainExceptionValidation.GetErrors(entryDate.HasValue && entryDate > DateTime.UtcNow, "EntryDate cannot be in the future");
        DomainExceptionValidation.GetErrors(hourlyRate <= 0, "HourlyRate must be greater than zero");
    }
}