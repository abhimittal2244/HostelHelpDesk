using HostelHelpDesk.Application.DTO;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IComplaintTypeWorkerTypeService
    {
        Task CreateMappingAsync(ComplaintTypeWorkerTypeDto dto);
        Task<List<ComplaintTypeWorkerTypeDto>> GetAllMappingsAsync();
        Task<bool> MappingExistsAsync(string complaintTypeName, string workerTypeName);
        Task<bool> DeleteMappingAsync(int complaintTypeId, int workerTypeId);
        Task<List<WorkerTypeResponseDto>> GetAllWorkerTypesAsync();
        Task<List<ComplaintTypeResponseDto>> GetAllComplaintTypesAsync();
    }
}
