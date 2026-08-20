using Mapster;
using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Service.Mappings;

public class VehicleMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Vehicle, VehicleDTO>();

        config.NewConfig<VehicleDTO, Vehicle>()
            .ConstructUsing(src => new Vehicle(
                src.Id,
                src.VehicleType,
                src.Brand,
                src.Model,
                src.Color,
                src.VehicleYear,
                src.Notes
            ));
    }
}