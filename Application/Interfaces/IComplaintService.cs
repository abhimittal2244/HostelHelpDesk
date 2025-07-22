using HostelHelpDesk.Application.DTO;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IComplaintService
    {
        Task<IEnumerable<ComplaintResponseDto>> GetAllComplaintsAsync();
        Task<ComplaintResponseDto> GetComplaintByIdAsync(int complaintId);
        Task<IEnumerable<ComplaintResponseDto>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<ComplaintResponseDto>> GetByWorkerIdAsync(int workerId);
        Task<IEnumerable<ComplaintResponseDto>> GetByCaretakerIdAsync(int caretakerId);
        Task<bool> DeleteComplaintAsync(int complaintId);
        Task<ComplaintResponseDto> AddComplaintAsync(ComplaintRequestDto dto);
    }
}
