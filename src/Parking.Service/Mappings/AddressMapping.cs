using Mapster;
using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Service.Mappings;

public class AddressMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressDTO>();

        config.NewConfig<AddressDTO, Address>()
            .ConstructUsing(src => new Address(
                src.Id,
                src.Street,
                src.Number,
                src.Complement,
                src.Neighborhood,
                src.FederativeUnit,
                src.City,
                src.ZipCode
            ));
    }
}