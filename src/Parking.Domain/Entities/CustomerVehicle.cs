using Parking.Domain.Common;
using Parking.Domain.Validations;

namespace Parking.Domain.Entities;

public class CustomerVehicle : Entity
{
    public int? CustomerId { get; private set; }
    public int? VehicleId { get; private set; }

    public Customer Customer { get; private set; }
    public Vehicle Vehicle { get; private set; }
    public ICollection<Stay> Stays { get; private set; } = new List<Stay>();

    public CustomerVehicle(int id, int? customerId, int? vehicleId)
    {
        ValidateDomain(customerId, vehicleId);

        Id = id;
        CustomerId = customerId;
        VehicleId = vehicleId;
    }

    private void ValidateDomain(int? customerId, int? vehicleId)
    {
        DomainExceptionValidation.GetErrors(!customerId.HasValue || customerId <= 0, "CustomerId is required and must be greater than zero");
        DomainExceptionValidation.GetErrors(!vehicleId.HasValue || vehicleId <= 0, "VehicleId is required and must be greater than zero");
    }
}