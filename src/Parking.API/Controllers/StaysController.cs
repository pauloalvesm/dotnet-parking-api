using Microsoft.AspNetCore.Mvc;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaysController : ControllerBase
{
    private readonly IStayService _stayService;

    public StaysController(IStayService stayService)
    {
        _stayService = stayService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetAll()
    {
        var stays = await _stayService.GetAllStays();
        return Ok(stays);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StayDTO>> GetById(int id)
    {
        var stay = await _stayService.GetStayById(id);
        if (stay == null) return NotFound();

        return Ok(stay);
    }

    [HttpPost]
    public async Task<ActionResult<StayDTO>> Create([FromBody] StayDTO stayDto)
    {
        var createdStay = await _stayService.CreateStay(stayDto);
        return CreatedAtAction(nameof(GetById), new { id = createdStay.Id }, createdStay);
    }

    [HttpPut("{id:int}/complete")]
    public async Task<ActionResult<StayDTO>> Complete(int id, [FromBody] DateTime exitDate)
    {
        var completedStay = await _stayService.CompleteStay(id, exitDate);
        return Ok(completedStay);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<ActionResult<StayDTO>> Cancel(int id)
    {
        var cancelledStay = await _stayService.CancelStay(id);
        return Ok(cancelledStay);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _stayService.DeleteStay(id);
        return NoContent();
    }
}