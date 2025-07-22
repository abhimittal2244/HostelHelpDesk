using HostelHelpDesk.Application.DTO;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface ITimeslotService
    {
        Task<IEnumerable<TimeslotResponseDto>> GetAllTimeslotsAsync();
        Task<TimeslotResponseDto?> GetTimeslotByIdAsync(int id);
        Task<TimeslotResponseDto> AddTimeslotAsync(TimeslotRequestDto dto);
        Task<bool> DeleteTimeslotAsync(int id);
    }
}
