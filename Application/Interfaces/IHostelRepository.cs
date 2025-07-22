using Microsoft.AspNetCore.Mvc;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface IHostelRepository
    {
        Task<ActionResult<Hostel>> AddAsync(Hostel hostel);
        Task <IEnumerable<Hostel>> GetAllAsync();
        Task<ActionResult<Hostel>> DeleteAsync(int id);
        Task <Hostel> FindAsync(int id);
        Task <IEnumerable<Hostel>> FindByNameAsync(string name);
    }
}
