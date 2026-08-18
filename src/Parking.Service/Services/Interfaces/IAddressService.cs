using Parking.Service.DTOs;

namespace Parking.Service.Interfaces;

public interface IAddressService
{
    Task<IEnumerable<AddressDTO>> GetAllAddresses();
    Task<AddressDTO> GetAddressById(int id);
    Task<AddressDTO> CreateAddress(AddressDTO addressDto);
    Task<AddressDTO> UpdateAddress(AddressDTO addressDto);
    Task DeleteAddress(int id);
}