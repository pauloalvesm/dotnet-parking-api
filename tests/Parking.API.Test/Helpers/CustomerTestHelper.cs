using Parking.Service.DTOs;

namespace Parking.API.Test.Helpers;

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
            Email = "john.doe@example.com",
            AddressId = 1,
            Address = new AddressDTO
            {
                Id = 1,
                Street = "Main Street",
                Number = "100",
                Complement = "Apt 1",
                Neighborhood = "Downtown",
                FederativeUnit = "NY",
                City = "New York",
                ZipCode = "12345-678"
            }
        };
    }
}