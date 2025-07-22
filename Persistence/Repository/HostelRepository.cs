using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.Persistence.Repository
{
    public class HostelRepository
    {
        private readonly HostelComplaintsDB _context;

        public HostelRepository(HostelComplaintsDB context)
        {
            _context = context;
        }

        public async Task<Hostel> AddAsync(Hostel request)
        {
            var hostel = await _context.Hostels.AddAsync(request);
            await _context.SaveChangesAsync();
            return hostel.Entity;
        }

        public async Task<Hostel> DeleteAsync(int id)
        {
            var hostel = await _context.Hostels.FindAsync(id);
            if (hostel != null)
            {
                var deleted = _context.Hostels.Remove(hostel);
                await _context.SaveChangesAsync();
                return deleted.Entity;
            }
            return null;
        }

        public async Task<Hostel?> FindAsync(int id)
        {
            return await _context.Hostels.FindAsync(id);
        }

        public async Task<IEnumerable<Hostel>> FindAllByNameAsync(string name)
        {
            return await _context.Hostels.Where(x => x.HostelName == name).ToListAsync();
        }

        public async Task<IEnumerable<Hostel>> GetAllAsync()
        {
            return await _context.Hostels.ToListAsync();
        }

        public async Task<bool> IsExistAsync(string hostelName)
        {
            return await _context.Hostels.AnyAsync(h => h.HostelName == hostelName);
        }

    }

}
