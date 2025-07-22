using HostelHelpDesk.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.Persistence.Repository
{
    public class ComplaintTypeRepository : IComplaintTypeRepository
    {
        private readonly HostelComplaintsDB _context;

        public ComplaintTypeRepository(HostelComplaintsDB context)
        {
            _context = context;
        }
        public async Task<ComplaintType> AddAsync(ComplaintType dto)
        {
            var saved = await _context.ComplaintTypes.AddAsync(dto);
            await _context.SaveChangesAsync();
            return saved.Entity;
        }

        public async Task<IEnumerable<ComplaintType>> GetAllAsync()
        {
            return await _context.ComplaintTypes.ToListAsync();
        }
    }
}
