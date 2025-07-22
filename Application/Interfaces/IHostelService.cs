using HostelHelpDesk.Application.DTO;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IHostelService
    {
        Task<HostelResponseDto> AddHostelAsync(HostelRequestDto dto);
        Task DeleteHostelAsync(int hid);
    }
}
