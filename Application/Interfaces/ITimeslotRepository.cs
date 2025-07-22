using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.Interfaces
{
    public interface ITimeslotRepository
    {
        Task<Timeslot> AddAsync(Timeslot timeslot);
        Task<Timeslot?> GetByIdAsync(int id);
        Task<IEnumerable<Timeslot>> GetAllAsync();
        Task<Timeslot?> UpdateAsync(Timeslot timeslot);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(TimeOnly startTime, TimeOnly endTime);
    }
}
