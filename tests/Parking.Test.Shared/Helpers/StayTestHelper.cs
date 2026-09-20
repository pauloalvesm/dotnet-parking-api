using System.Reflection;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Service.DTOs;

namespace Parking.Test.Shared.Helpers;

public static class StayTestHelper
{
    public static StayDTO CreateValidStayDTO(int id = 1)
    {
        return new StayDTO
        {
            Id = id,
            CustomerVehicleId = 1,
            LicensePlate = "ABC1D23",
            EntryDate = DateTime.Now.AddHours(-2),
            ExitDate = null,
            HourlyRate = 10.00m,
            TotalAmount = null,
            StayStatus = (StayStatus)1,
            CustomerVehicle = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1)
        };
    }

    public static Stay CreateValidStayEntity(int id = 1)
    {
        var stay = (Stay)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Stay));

        SetPropertyIfExists(stay, nameof(Stay.Id), id);
        SetPropertyIfExists(stay, nameof(Stay.CustomerVehicleId), 1);
        SetPropertyIfExists(stay, nameof(Stay.LicensePlate), "ABC1D23");
        SetPropertyIfExists(stay, nameof(Stay.EntryDate), DateTime.Now.AddHours(-2));
        SetPropertyIfExists(stay, nameof(Stay.HourlyRate), 10.00m);
        SetPropertyIfExists(stay, nameof(Stay.StayStatus), (StayStatus)1);
        SetPropertyIfExists(stay, nameof(Stay.CustomerVehicle), CustomerVehicleTestHelper.CreateValidCustomerVehicleEntity(1));

        return stay;
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