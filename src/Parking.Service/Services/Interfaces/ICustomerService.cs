using Parking.Service.DTOs;

namespace Parking.Service.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDTO>> GetAllCustomers();
    Task<CustomerDTO> GetCustomerById(int id);
    Task<CustomerDTO> CreateCustomer(CustomerDTO customerDto);
    Task<CustomerDTO> UpdateCustomer(CustomerDTO customerDto);
    Task DeleteCustomer(int id);
}