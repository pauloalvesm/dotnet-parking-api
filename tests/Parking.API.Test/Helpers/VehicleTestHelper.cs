using Parking.Domain.Enums;
using Parking.Service.DTOs;

namespace Parking.API.Test.Helpers;

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
            Notes = "Standard sedan in good condition"
        };
    }
}