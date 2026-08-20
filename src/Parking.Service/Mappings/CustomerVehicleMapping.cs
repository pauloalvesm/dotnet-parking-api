using Mapster;
using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Service.Mappings;

public class CustomerVehicleMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CustomerVehicle, CustomerVehicleDTO>();

        config.NewConfig<CustomerVehicleDTO, CustomerVehicle>()
            .ConstructUsing(src => new CustomerVehicle(
                src.Id,
                src.CustomerId,
                src.VehicleId
            ));
    }
}