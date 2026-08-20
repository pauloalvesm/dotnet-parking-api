using Mapster;
using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Service.Mappings;

public class StayMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Stay, StayDTO>();

        config.NewConfig<StayDTO, Stay>()
            .ConstructUsing(src => new Stay(
                src.Id,
                src.CustomerVehicleId,
                src.LicensePlate,
                src.EntryDate,
                src.ExitDate,
                src.HourlyRate,
                src.TotalAmount,
                src.StayStatus,
                null
            ));
    }
}