using HostelHelpDesk.Application.DTO;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IRoomService
    {
        Task<RoomResponseDto> AddRoomAsync(RoomRequestDto dto);
        Task<IEnumerable<RoomResponseDto>> GetAllRoomsAsync();
        Task<IEnumerable<RoomResponseDto>> GetRoomsByHostelIdAsync(int hostelId);
        Task<RoomResponseDto> GetRoomByIdAsync(int id);
        Task<bool> IsRoomExistAsync(string roomNumber, int hostelId);
        Task<bool> DeleteRoomAsync(int id);
    }
}
