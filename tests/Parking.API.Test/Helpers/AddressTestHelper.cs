using Parking.Service.DTOs;

namespace Parking.API.Test.Helpers;

public static class AddressTestHelper
{
    public static AddressDTO CreateValidAddressDTO(int id = 1)
    {
        return new AddressDTO
        {
            Id = id,
            Street = "Flower Street",
            Number = "123",
            Complement = "Apt 101",
            Neighborhood = "Downtown",
            FederativeUnit = "NY",
            City = "New York",
            ZipCode = "12345-678"
        };
    }
}