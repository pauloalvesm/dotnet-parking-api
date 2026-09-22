using Parking.Service.DTOs;

namespace Parking.Service.Interfaces;

public interface IPdfService
{
    byte[] GenerateStayPdf(StayDTO stayDto);
}