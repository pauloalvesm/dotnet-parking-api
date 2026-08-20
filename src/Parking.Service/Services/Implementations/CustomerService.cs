using MapsterMapper;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Service.Implementations;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
    }

    public async Task<CustomerDTO> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return _mapper.Map<CustomerDTO>(customer);
    }

    public async Task<CustomerDTO> CreateCustomer(CustomerDTO customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);
        var createdCustomer = await _customerRepository.AddAsync(customer);
        return _mapper.Map<CustomerDTO>(createdCustomer);
    }

    public async Task<CustomerDTO> UpdateCustomer(CustomerDTO customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);
        var updatedCustomer = await _customerRepository.UpdateAsync(customer);
        return _mapper.Map<CustomerDTO>(updatedCustomer);
    }

    public async Task DeleteCustomer(int id)
    {
        await _customerRepository.DeleteAsync(id);
    }
}