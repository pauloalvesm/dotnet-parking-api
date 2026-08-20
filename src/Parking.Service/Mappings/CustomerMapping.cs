using Mapster;
using Parking.Domain.Entities;
using Parking.Service.DTOs;

namespace Parking.Service.Mappings;

public class CustomerMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, CustomerDTO>();

        config.NewConfig<CustomerDTO, Customer>()
            .ConstructUsing(src => new Customer(
                src.Id,
                src.Name,
                src.BirthDate,
                src.Cpf,
                src.Phone,
                src.Email,
                src.AddressId
            ));
    }
}