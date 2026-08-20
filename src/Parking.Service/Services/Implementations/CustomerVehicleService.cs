using MapsterMapper;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Service.Implementations;

public class CustomerVehicleService : ICustomerVehicleService
{
    private readonly ICustomerVehicleRepository _customerVehicleRepository;
    private readonly IMapper _mapper;

    public CustomerVehicleService(ICustomerVehicleRepository customerVehicleRepository, IMapper mapper)
    {
        _customerVehicleRepository = customerVehicleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerVehicleDTO>> GetAllCustomerVehicles()
    {
        var customerVehicles = await _customerVehicleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerVehicleDTO>>(customerVehicles);
    }

    public async Task<CustomerVehicleDTO> GetCustomerVehicleById(int id)
    {
        var customerVehicle = await _customerVehicleRepository.GetByIdAsync(id);
        return _mapper.Map<CustomerVehicleDTO>(customerVehicle);
    }

    public async Task<CustomerVehicleDTO> CreateCustomerVehicle(CustomerVehicleDTO customerVehicleDto)
    {
        var customerVehicle = _mapper.Map<CustomerVehicle>(customerVehicleDto);
        var createdCustomerVehicle = await _customerVehicleRepository.AddAsync(customerVehicle);
        return _mapper.Map<CustomerVehicleDTO>(createdCustomerVehicle);
    }

    public async Task<CustomerVehicleDTO> UpdateCustomerVehicle(CustomerVehicleDTO customerVehicleDto)
    {
        var customerVehicle = _mapper.Map<CustomerVehicle>(customerVehicleDto);
        var updatedCustomerVehicle = await _customerVehicleRepository.UpdateAsync(customerVehicle);
        return _mapper.Map<CustomerVehicleDTO>(updatedCustomerVehicle);
    }

    public async Task DeleteCustomerVehicle(int id)
    {
        await _customerVehicleRepository.DeleteAsync(id);
    }
}