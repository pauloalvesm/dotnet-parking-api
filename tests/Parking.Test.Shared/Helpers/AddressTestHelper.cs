using System.Reflection;
using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Test.Shared.Helpers;

public static class AddressTestHelper
{
    public static AddressDTO CreateValidAddressDTO(int id = 1)
    {
        return new AddressDTO
        {
            Id = id,
            Street = "Main Street",
            Number = "123",
            Complement = "Suite 101",
            Neighborhood = "Downtown",
            FederativeUnit = "SP",
            City = "São Paulo",
            ZipCode = "12345-678"
        };
    }

    public static Address CreateValidAddressEntity(int id = 1)
    {
        var address = (Address)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Address));

        SetPropertyIfExists(address, nameof(Address.Id), id);
        SetPropertyIfExists(address, nameof(Address.Street), "Main Street");
        SetPropertyIfExists(address, nameof(Address.Number), "123");
        SetPropertyIfExists(address, nameof(Address.Complement), "Suite 101");
        SetPropertyIfExists(address, nameof(Address.Neighborhood), "Downtown");
        SetPropertyIfExists(address, nameof(Address.FederativeUnit), "SP");
        SetPropertyIfExists(address, nameof(Address.City), "São Paulo");
        SetPropertyIfExists(address, nameof(Address.ZipCode), "12345-678");

        return address;
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