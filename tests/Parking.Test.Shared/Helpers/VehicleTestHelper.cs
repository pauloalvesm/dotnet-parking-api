using System.Reflection;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Service.DTOs;

namespace Parking.Test.Shared.Helpers;

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
        SetPropertyIfExists(vehicle, nameof(Vehicle.VehicleType), (VehicleType)1);
        SetPropertyIfExists(vehicle, nameof(Vehicle.Brand), "Toyota");
        SetPropertyIfExists(vehicle, nameof(Vehicle.Model), "Corolla");
        SetPropertyIfExists(vehicle, nameof(Vehicle.Color), "Black");
        SetPropertyIfExists(vehicle, nameof(Vehicle.VehicleYear), 2022);
        SetPropertyIfExists(vehicle, nameof(Vehicle.Notes), "Test vehicle notes");

        return vehicle;
    }

    private static void SetPropertyIfExists(object obj, string propertyName, object value)
    {
        var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(obj, value);
        }
    }
}