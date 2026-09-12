using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Service.DTOs;

namespace Parking.Service.Test.Helpers;

public static class VehicleTestHelper
{
    public static VehicleDTO CreateValidVehicleDTO(int id = 1)
    {
        return new VehicleDTO
        {
            Id = id,
            VehicleType = (VehicleType)1,
            Brand = "Toyota",
            Model = "Corolla",
            Color = "Black",
            VehicleYear = 2022,
            Notes = "Test vehicle notes"
        };
    }

    public static Vehicle CreateValidVehicleEntity(int id = 1)
    {
        var vehicle = (Vehicle)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Vehicle));

        SetPropertyIfExists(vehicle, nameof(Vehicle.Id), id);
        SetPropertyIfExists(vehicle, "VehicleType", (VehicleType)1);
        SetPropertyIfExists(vehicle, "Brand", "Toyota");
        SetPropertyIfExists(vehicle, "Model", "Corolla");
        SetPropertyIfExists(vehicle, "Color", "Black");
        SetPropertyIfExists(vehicle, "VehicleYear", 2022);
        SetPropertyIfExists(vehicle, "Notes", "Test vehicle notes");

        return vehicle;
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