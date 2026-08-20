using MapsterMapper;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Service.Implementations;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public AddressService(IAddressRepository addressRepository, IMapper mapper)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AddressDTO>> GetAllAddresses()
    {
        var addresses = await _addressRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AddressDTO>>(addresses);
    }

    public async Task<AddressDTO> GetAddressById(int id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        return _mapper.Map<AddressDTO>(address);
    }

    public async Task<AddressDTO> CreateAddress(AddressDTO addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);
        var createdAddress = await _addressRepository.AddAsync(address);
        return _mapper.Map<AddressDTO>(createdAddress);
    }

    public async Task<AddressDTO> UpdateAddress(AddressDTO addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);
        var updatedAddress = await _addressRepository.UpdateAsync(address);
        return _mapper.Map<AddressDTO>(updatedAddress);
    }

    public async Task DeleteAddress(int id)
    {
        await _addressRepository.DeleteAsync(id);
    }
}