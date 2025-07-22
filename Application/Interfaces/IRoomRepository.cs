using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> AddAsync(Room room);
        Task<Room> FindAsync(int id);
        Task<IEnumerable<Room>> GetAllAsync();
        Task<IEnumerable<Room>> FindByHostelIdAsync(int hostelId);
        Task<Room> DeleteAsync(int id);
        Task<bool> IsRoomExistAsync(string roomNumber, int hostelId);
    }
}
