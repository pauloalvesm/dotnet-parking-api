using Parking.Domain.Enums;
using Parking.Service.DTOs;

namespace Parking.API.Test.Helpers;

public static class CustomerVehicleTestHelper
{
    public static CustomerVehicleDTO CreateValidCustomerVehicleDTO(int id = 1)
    {
        return new CustomerVehicleDTO
        {
            Id = id,
            CustomerId = 1,
            VehicleId = 1,
            Customer = new CustomerDTO
            {
                Id = 1,
                Name = "John Doe",
                BirthDate = new DateOnly(1990, 1, 1),
                Cpf = "12345678901",
                Phone = "11999999999",
                Email = "john.doe@example.com",
                AddressId = 1
            },
            Vehicle = new VehicleDTO
            {
                Id = 1,
                VehicleType = (VehicleType)1,
                Brand = "Toyota",
                Model = "Corolla",
                Color = "Black",
                VehicleYear = 2022,
                Notes = "Standard sedan"
            }
        };
    }
}