using Parking.Service.DTOs;

namespace Parking.Service.Interfaces;

public interface ICustomerVehicleService
{
    Task<IEnumerable<CustomerVehicleDTO>> GetAllCustomerVehicles();
    Task<CustomerVehicleDTO> GetCustomerVehicleById(int id);
    Task<CustomerVehicleDTO> CreateCustomerVehicle(CustomerVehicleDTO customerVehicleDto);
    Task<CustomerVehicleDTO> UpdateCustomerVehicle(CustomerVehicleDTO customerVehicleDto);
    Task DeleteCustomerVehicle(int id);
}