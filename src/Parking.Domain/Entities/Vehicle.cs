using Parking.Domain.Common;
using Parking.Domain.Enums;
using Parking.Domain.Validations;

namespace Parking.Domain.Entities;

public class Vehicle : Entity
{
    public VehicleType VehicleType { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public string Color { get; private set; }
    public int? VehicleYear { get; private set; }
    public string Notes { get; private set; }

    public ICollection<CustomerVehicle> CustomerVehicles { get; private set; } = new List<CustomerVehicle>();

    public Vehicle(int id,
                   VehicleType vehicleType,
                   string brand,
                   string model,
                   string color,
                   int? vehicleYear,
                   string notes)
    {
        ValidateDomain(brand, model, color, vehicleYear, notes);

        Id = id;
        VehicleType = vehicleType;
        Brand = brand;
        Model = model;
        Color = color;
        VehicleYear = vehicleYear;
        Notes = notes;
    }

    private void ValidateDomain(string brand,
                               string model,
                               string color,
                               int? vehicleYear,
                               string notes)
    {
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(brand), "Brand is required");
        DomainExceptionValidation.GetErrors(brand.Length > 50, "Brand cannot exceed 50 characters");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(model), "Model is required");
        DomainExceptionValidation.GetErrors(model.Length > 50, "Model cannot exceed 50 characters");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(color), "Color is required");
        DomainExceptionValidation.GetErrors(color.Length > 50, "Color cannot exceed 50 characters");

        if (vehicleYear.HasValue)
        {
            DomainExceptionValidation.GetErrors(vehicleYear < 1900 || vehicleYear > 2100, "VehicleYear must be between 1900 and 2100");
        }

        if (!string.IsNullOrEmpty(notes))
        {
            DomainExceptionValidation.GetErrors(notes.Length > 200, "Notes cannot exceed 200 characters");
        }
    }
}