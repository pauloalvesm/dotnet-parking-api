using Parking.Service.DTOs;

namespace Parking.Service.Interfaces;

public interface IStayService
{
    Task<IEnumerable<StayDTO>> GetAllStays();
    Task<StayDTO> GetStayById(int id);
    Task<StayDTO> CreateStay(StayDTO stayDto);
    Task<StayDTO> CompleteStay(int id, DateTime exitDate);
    Task<StayDTO> CancelStay(int id);
    Task DeleteStay(int id);
}