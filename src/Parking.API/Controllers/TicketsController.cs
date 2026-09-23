using Microsoft.AspNetCore.Mvc;
using Parking.Service.Interfaces;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/stays/{stayId:int}/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IStayService _stayService;
    private readonly IPdfService _pdfService;

    public TicketsController(IStayService stayService, IPdfService pdfService)
    {
        _stayService = stayService;
        _pdfService = pdfService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTicket(int stayId)
    {
        var stayDto = await _stayService.GetStayById(stayId);

        if (stayDto == null)
        {
            return NotFound($"Stay with ID {stayId} not found.");
        }

        var pdfBytes = _pdfService.GenerateStayPdf(stayDto);

        return File(pdfBytes, "application/pdf", $"Stay_{stayId}.pdf");
    }
}