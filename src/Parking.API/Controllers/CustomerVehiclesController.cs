using Microsoft.AspNetCore.Mvc;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerVehiclesController : ControllerBase
{
    private readonly ICustomerVehicleService _customerVehicleService;

    public CustomerVehiclesController(ICustomerVehicleService customerVehicleService)
    {
        _customerVehicleService = customerVehicleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerVehicleDTO>>> GetAll()
    {
        var customerVehicles = await _customerVehicleService.GetAllCustomerVehicles();
        return Ok(customerVehicles);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerVehicleDTO>> GetById(int id)
    {
        var customerVehicle = await _customerVehicleService.GetCustomerVehicleById(id);
        if (customerVehicle == null) return NotFound();

        return Ok(customerVehicle);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerVehicleDTO>> Create([FromBody] CustomerVehicleDTO customerVehicleDto)
    {
        var createdCustomerVehicle = await _customerVehicleService.CreateCustomerVehicle(customerVehicleDto);
        return CreatedAtAction(nameof(GetById), new { id = createdCustomerVehicle.Id }, createdCustomerVehicle);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CustomerVehicleDTO>> Update(int id, [FromBody] CustomerVehicleDTO customerVehicleDto)
    {
        if (id != customerVehicleDto.Id) return BadRequest("ID Mismatch");

        var updatedCustomerVehicle = await _customerVehicleService.UpdateCustomerVehicle(customerVehicleDto);
        return Ok(updatedCustomerVehicle);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerVehicleService.DeleteCustomerVehicle(id);
        return NoContent();
    }
}