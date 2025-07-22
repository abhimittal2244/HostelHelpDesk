using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.Persistence.Repository
{
    public class TimeslotRepository : ITimeslotRepository
    {
        private readonly HostelComplaintsDB _context;

        public TimeslotRepository(HostelComplaintsDB context)
        {
            _context = context;
        }

        public async Task<Timeslot> AddAsync(Timeslot timeslot)
        {
            var result = await _context.Timeslots.AddAsync(timeslot);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Timeslot?> GetByIdAsync(int id)
        {
            return await _context.Timeslots.FindAsync(id);
        }

        public async Task<IEnumerable<Timeslot>> GetAllAsync()
        {
            return await _context.Timeslots.ToListAsync();
        }

        public async Task<Timeslot?> UpdateAsync(Timeslot timeslot)
        {
            var existing = await _context.Timeslots.FindAsync(timeslot.Id);
            if (existing == null) return null;

            existing.StartTime = timeslot.StartTime;
            existing.EndTime = timeslot.EndTime;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Timeslots.FindAsync(id);
            if (existing == null) return false;

            _context.Timeslots.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.Timeslots.AnyAsync(t =>
                t.StartTime == startTime && t.EndTime == endTime);
        }
    }
}
