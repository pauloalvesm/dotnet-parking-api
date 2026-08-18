using Parking.Service.DTOs;

namespace Parking.Service.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDTO>> GetAllVehicles();
    Task<VehicleDTO> GetVehicleById(int id);
    Task<VehicleDTO> CreateVehicle(VehicleDTO vehicleDto);
    Task<VehicleDTO> UpdateVehicle(VehicleDTO vehicleDto);
    Task DeleteVehicle(int id);
}