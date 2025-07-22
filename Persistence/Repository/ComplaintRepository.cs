using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Shared.Enums;

namespace HostelHelpDesk.Persistence.Repository
{
    public class ComplaintRepository
    {
        private readonly HostelComplaintsDB _context;

        public ComplaintRepository(HostelComplaintsDB context)
        {
            _context = context;
        }

        public async Task<Complaint> AddAsync(Complaint complaint)
        {
            var result = await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Complaint?> GetByIdAsync(int id)
        {
            return await _context.Complaints
                .Include(c => c.Student)
                .Include(c => c.Caretaker)
                .Include(c => c.Timeslot)
                .Include(c => c.Hostel)
                .Include(c => c.Room)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Complaint>> GetAllAsync()
        {
            return await _context.Complaints
                .Include(c => c.Student)
                .Include(c => c.Caretaker)
                .Include(c => c.Timeslot)
                .Include(c => c.Hostel)
                .Include(c => c.Room)
                .ToListAsync();
        }

        public async Task<IEnumerable<Complaint>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Complaints
                .Where(c => c.Student.Id == studentId)
                .Include(c => c.Caretaker)
                .Include(c => c.Timeslot)
                .Include(c => c.Hostel)
                .Include(c => c.Room)
                .ToListAsync();
        }

        public async Task<IEnumerable<Complaint>> GetByCaretakerIdAsync(int caretakerId)
        {
            return await _context.Complaints
                .Where(c => c.Caretaker.Id == caretakerId)
                .Include(c => c.Student)
                .Include(c => c.Timeslot)
                .Include(c => c.Hostel)
                .Include(c => c.Room)
                .ToListAsync();
        }

        public async Task<IEnumerable<Complaint>> GetByWorkerIdAsync(int workerId)
        {
            return await _context.Complaints
                .Where(c => c.Worker.Id == workerId)
                .Include(c => c.Student)
                .Include(c => c.Caretaker)
                .Include(c => c.Timeslot)
                .Include(c => c.Hostel)
                .Include(c => c.Room)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int complaintId, ComplaintStatus newStatus)
        {
            var complaint = await _context.Complaints.FindAsync(complaintId);
            if (complaint == null) return false;

            complaint.Status = newStatus;
            _context.Complaints.Update(complaint);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int complaintId)
        {
            var complaint = await _context.Complaints.FindAsync(complaintId);
            var deleted = _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
            return deleted.Entity != null;
        }
    }
}
