using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IComplaintTypeWorkerTypeRepository
    {
        Task CreateAsync(ComplaintTypeWorkerTypeDto dto);
        Task<List<ComplaintTypeWorkerTypeDto>> GetAllAsync();
        Task<bool> MappingExistsAsync(int complaintTypeId, int workerTypeId);
        Task<bool> DeleteAsync(string complaintTypeName, string workerTypeName);
        Task<List<WorkerType>> GetAllWorkerTypesAsync();
        Task<List<ComplaintType>> GetAllComplaintTypesAsync();
    }
}
