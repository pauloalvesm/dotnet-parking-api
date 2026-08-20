using MapsterMapper;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Interfaces;

namespace Parking.Service.Implementations;

public class StayService : IStayService
{
    private readonly IStayRepository _stayRepository;
    private readonly IMapper _mapper;

    public StayService(IStayRepository stayRepository, IMapper mapper)
    {
        _stayRepository = stayRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StayDTO>> GetAllStays()
    {
        var stays = await _stayRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StayDTO>>(stays);
    }

    public async Task<StayDTO> GetStayById(int id)
    {
        var stay = await _stayRepository.GetByIdAsync(id);
        return _mapper.Map<StayDTO>(stay);
    }

    public async Task<StayDTO> CreateStay(StayDTO stayDto)
    {
        var stay = _mapper.Map<Stay>(stayDto);

        var createdStay = await _stayRepository.AddAsync(stay);
        return _mapper.Map<StayDTO>(createdStay);
    }

    public async Task<StayDTO> CompleteStay(int id, DateTime exitDate)
    {
        var stay = await _stayRepository.GetByIdAsync(id);

        if (stay == null) 
        {
            throw new KeyNotFoundException($"Stay with ID {id} not found.");
        }

        stay.CompleteStay(exitDate);

        var updatedStay = await _stayRepository.UpdateAsync(stay);
        return _mapper.Map<StayDTO>(updatedStay);
    }

    public async Task<StayDTO> CancelStay(int id)
    {
        var stay = await _stayRepository.GetByIdAsync(id);

        if (stay == null) 
        {
            throw new KeyNotFoundException($"Stay with ID {id} not found.");
        }
            
        stay.CancelStay();

        var updatedStay = await _stayRepository.UpdateAsync(stay);
        return _mapper.Map<StayDTO>(updatedStay);
    }

    public async Task DeleteStay(int id)
    {
        var stay = await _stayRepository.GetByIdAsync(id);

        if (stay == null) 
        {
            throw new KeyNotFoundException($"Stay with ID {id} not found.");
        }

        await _stayRepository.DeleteAsync(id);
    }
}