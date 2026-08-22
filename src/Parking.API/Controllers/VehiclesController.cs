using Microsoft.AspNetCore.Mvc;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetAll()
    {
        var vehicles = await _vehicleService.GetAllVehicles();
        return Ok(vehicles);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleDTO>> GetById(int id)
    {
        var vehicle = await _vehicleService.GetVehicleById(id);
        if (vehicle == null) return NotFound();

        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDTO>> Create([FromBody] VehicleDTO vehicleDto)
    {
        var createdVehicle = await _vehicleService.CreateVehicle(vehicleDto);
        return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, createdVehicle);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VehicleDTO>> Update(int id, [FromBody] VehicleDTO vehicleDto)
    {
        if (id != vehicleDto.Id) return BadRequest("ID Mismatch");

        var updatedVehicle = await _vehicleService.UpdateVehicle(vehicleDto);
        return Ok(updatedVehicle);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _vehicleService.DeleteVehicle(id);
        return NoContent();
    }
}