using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IComplaintTypeRepository
    {
        Task<ComplaintType> AddAsync(ComplaintType dto);
        Task<IEnumerable<ComplaintType>> GetAllAsync();
    }
}
