using MapsterMapper;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Service.Implementations;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehicleDTO>> GetAllVehicles()
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);
    }

    public async Task<VehicleDTO> GetVehicleById(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        return _mapper.Map<VehicleDTO>(vehicle);
    }

    public async Task<VehicleDTO> CreateVehicle(VehicleDTO vehicleDto)
    {
        var vehicle = _mapper.Map<Vehicle>(vehicleDto);
        var createdVehicle = await _vehicleRepository.AddAsync(vehicle);
        return _mapper.Map<VehicleDTO>(createdVehicle);
    }

    public async Task<VehicleDTO> UpdateVehicle(VehicleDTO vehicleDto)
    {
        var vehicle = _mapper.Map<Vehicle>(vehicleDto);
        var updatedVehicle = await _vehicleRepository.UpdateAsync(vehicle);
        return _mapper.Map<VehicleDTO>(updatedVehicle);
    }

    public async Task DeleteVehicle(int id)
    {
        await _vehicleRepository.DeleteAsync(id);
    }
}