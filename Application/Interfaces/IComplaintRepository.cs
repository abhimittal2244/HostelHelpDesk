using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IComplaintRepository
    {

        Task<Complaint> AddAsync(Complaint complaint);
        Task<Complaint?> GetByIdAsync(int id);
        Task<IEnumerable<Complaint>> GetAllAsync();
        Task<IEnumerable<Complaint>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Complaint>> GetByCaretakerIdAsync(int caretakerId);
        Task<IEnumerable<Complaint>> GetByWorkerIdAsync(int workerId);
        Task<bool> UpdateStatusAsync(int complaintId, string newStatus);

    }
}
