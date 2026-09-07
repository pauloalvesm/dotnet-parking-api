using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Service.Test.Helpers;

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
        SetPropertyIfExists(customer, "Name", "John Doe");
        SetPropertyIfExists(customer, "BirthDate", new DateOnly(1990, 1, 1));
        SetPropertyIfExists(customer, "Cpf", "12345678901");
        SetPropertyIfExists(customer, "Phone", "11999999999");
        SetPropertyIfExists(customer, "Email", "john.doe@test.com");
        SetPropertyIfExists(customer, "AddressId", 1);

        return customer;
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