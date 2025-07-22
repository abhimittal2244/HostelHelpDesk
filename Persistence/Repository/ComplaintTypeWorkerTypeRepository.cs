using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.Persistence.Repository
{
    public class ComplaintTypeWorkerTypeRepository
    {
        private readonly HostelComplaintsDB _context;

        public ComplaintTypeWorkerTypeRepository(HostelComplaintsDB context)
        {
            _context = context;
        }

        public async Task CreateAsync(ComplaintTypeWorkerTypeDto dto)
        {
            var complaintType = await _context.ComplaintTypes
                .FirstOrDefaultAsync(ct => ct.Name == dto.ComplaintTypeName);

            if (complaintType == null)
            {
                complaintType = new ComplaintType { Name = dto.ComplaintTypeName };
                _context.ComplaintTypes.Add(complaintType);
                await _context.SaveChangesAsync();
            }

            var exists = await _context.WorkerTypes
                .AnyAsync(wt => wt.Name == dto.WorkerTypeName && wt.ComplaintTypeId == complaintType.Id);

            if (!exists)
            {
                var workerType = new WorkerType
                {
                    Name = dto.WorkerTypeName,
                    ComplaintTypeId = complaintType.Id
                };

                _context.WorkerTypes.Add(workerType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<WorkerType>> GetAllWorkerTypesAsync()
        {
            return await _context.WorkerTypes.ToListAsync();
        }

        public async Task<List<ComplaintType>> GetAllComplaintTypesAsync()
        {
            return await _context.ComplaintTypes.ToListAsync();
            
        }

        public async Task<List<ComplaintTypeWorkerTypeDto>> GetAllAsync()
        {
            return await _context.WorkerTypes
                .Include(wt => wt.ComplaintType)
                .Select(wt => new ComplaintTypeWorkerTypeDto
                {
                    ComplaintTypeName = wt.ComplaintType.Name,
                    WorkerTypeName = wt.Name
                })
                .ToListAsync();
        }

        public async Task<ComplaintType?> GetComplaintTypeByIdAsync(int complaintId)
        {
            return await _context.ComplaintTypes.FindAsync(complaintId);
        }

        public async Task<bool> MappingExistsAsync(string complaintTypeName, string workerTypeName)
        {
            return await _context.WorkerTypes
                .Include(wt => wt.ComplaintType)
                .AnyAsync(wt => wt.ComplaintType.Name == complaintTypeName && wt.Name == workerTypeName);
        }

        public async Task<bool> DeleteAsync(int complaintTypeId, int workerTypeId)
        {
            var entity = await _context.WorkerTypes
                .Include(wt => wt.ComplaintType)
                .FirstOrDefaultAsync(wt => wt.ComplaintType.Id == complaintTypeId && wt.Id == workerTypeId);

            if (entity != null)
            {
                _context.WorkerTypes.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
