using System.Reflection;
using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Test.Shared.Helpers;

public static class CustomerTestHelper
{
    public static CustomerDTO CreateValidCustomerDTO(int id = 1)
    {
        return new CustomerDTO
        {
            Id = id,
            Name = "John Doe",
            BirthDate = new DateOnly(1990, 1, 1),
            Cpf = "12345678901",
            Phone = "11999999999",
            Email = "john.doe@test.com",
            AddressId = 1,
            Address = AddressTestHelper.CreateValidAddressDTO(1)
        };
    }

    public static Customer CreateValidCustomerEntity(int id = 1)
    {
        var customer = (Customer)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Customer));

        SetPropertyIfExists(customer, nameof(Customer.Id), id);
        SetPropertyIfExists(customer, nameof(Customer.Name), "John Doe");
        SetPropertyIfExists(customer, nameof(Customer.BirthDate), new DateOnly(1990, 1, 1));
        SetPropertyIfExists(customer, nameof(Customer.Cpf), "12345678901");
        SetPropertyIfExists(customer, nameof(Customer.Phone), "11999999999");
        SetPropertyIfExists(customer, nameof(Customer.Email), "john.doe@test.com");
        SetPropertyIfExists(customer, nameof(Customer.AddressId), 1);
        SetPropertyIfExists(customer, nameof(Customer.Address), AddressTestHelper.CreateValidAddressEntity(1));

        return customer;
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