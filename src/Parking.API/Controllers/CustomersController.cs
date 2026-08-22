using Microsoft.AspNetCore.Mvc;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAll()
    {
        var customers = await _customerService.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDTO>> GetById(int id)
    {
        var customer = await _customerService.GetCustomerById(id);
        if (customer == null) return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDTO>> Create([FromBody] CustomerDTO customerDto)
    {
        var createdCustomer = await _customerService.CreateCustomer(customerDto);
        return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CustomerDTO>> Update(int id, [FromBody] CustomerDTO customerDto)
    {
        if (id != customerDto.Id) return BadRequest("ID Mismatch");

        var updatedCustomer = await _customerService.UpdateCustomer(customerDto);
        return Ok(updatedCustomer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.DeleteCustomer(id);
        return NoContent();
    }
}