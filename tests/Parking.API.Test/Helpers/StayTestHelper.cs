using Parking.Domain.Enums;
using Parking.Service.DTOs;

namespace Parking.API.Test.Helpers;

public static class StayTestHelper
{
    public static StayDTO CreateValidStayDTO(int id = 1)
    {
        return new StayDTO
        {
            Id = id,
            CustomerVehicleId = 1,
            LicensePlate = "ABC-1234",
            EntryDate = new DateTime(2026, 1, 1, 10, 0, 0),
            ExitDate = null,
            HourlyRate = 10.00m,
            TotalAmount = null,
            StayStatus = (StayStatus)1,
            CustomerVehicle = CustomerVehicleTestHelper.CreateValidCustomerVehicleDTO(1)
        };
    }
}