using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.Persistence.Repository
{
    public class RoomRepository
    {
        private readonly HostelComplaintsDB _context;

        public RoomRepository(HostelComplaintsDB context)
        {
            _context = context;
        }

        public async Task<Room> AddAsync(Room room)
        {
            var added = await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<Room?> FindAsync(int id)
        {
            return await _context.Rooms.Include(r => r.Hostel).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.Include(r => r.Hostel).ToListAsync();
        }

        public async Task<IEnumerable<Room>> FindByHostelIdAsync(int hostelId)
        {
            return await _context.Rooms
                .Where(r => r.HostelId == hostelId)
                .Include(r => r.Hostel)
                .ToListAsync();
        }

        public async Task<Room> DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                var deleted = _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return deleted.Entity;
            }
            return null;
        }

        public async Task<bool> IsRoomExistAsync(string roomNo, int hostelId)
        {
            return await _context.Rooms.AnyAsync(r => r.RoomNo == roomNo && r.HostelId == hostelId);
        }
    }
}
