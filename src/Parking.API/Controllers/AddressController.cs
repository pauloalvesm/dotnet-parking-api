using Microsoft.AspNetCore.Mvc;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAll()
    {
        var addresses = await _addressService.GetAllAddresses();
        return Ok(addresses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AddressDTO>> GetById(int id)
    {
        var address = await _addressService.GetAddressById(id);
        if (address == null) return NotFound();

        return Ok(address);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDTO>> Create([FromBody] AddressDTO addressDto)
    {
        var createdAddress = await _addressService.CreateAddress(addressDto);
        return CreatedAtAction(nameof(GetById), new { id = createdAddress.Id }, createdAddress);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AddressDTO>> Update(int id, [FromBody] AddressDTO addressDto)
    {
        if (id != addressDto.Id) return BadRequest("ID Mismatch");

        var updatedAddress = await _addressService.UpdateAddress(addressDto);
        return Ok(updatedAddress);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _addressService.DeleteAddress(id);
        return NoContent();
    }
}