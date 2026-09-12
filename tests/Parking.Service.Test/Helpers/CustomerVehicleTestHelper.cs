using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Service.Test.Helpers;

public static class CustomerVehicleTestHelper
{
    public static CustomerVehicleDTO CreateValidCustomerVehicleDTO(int id = 1)
    {
        return new CustomerVehicleDTO
        {
            Id = id,
            CustomerId = 1,
            VehicleId = 1,
            Customer = CustomerTestHelper.CreateValidCustomerDTO(1),
            Vehicle = VehicleTestHelper.CreateValidVehicleDTO(1)
        };
    }

    public static CustomerVehicle CreateValidCustomerVehicleEntity(int id = 1)
    {
        var customerVehicle = (CustomerVehicle)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(CustomerVehicle));

        SetPropertyIfExists(customerVehicle, nameof(CustomerVehicle.Id), id);
        SetPropertyIfExists(customerVehicle, "CustomerId", 1);
        SetPropertyIfExists(customerVehicle, "VehicleId", 1);

        return customerVehicle;
    }

    private static void SetPropertyIfExists(object obj, string propertyName, object value)
    {
        var prop = obj.GetType().GetProperty(propertyName);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(obj, value);
        }
    }
}