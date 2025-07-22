using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Repository;

namespace HostelHelpDesk.Application.Services
{
    public class TimeslotService : ITimeslotService
    {
        private readonly ITimeslotRepository _repo;

        public TimeslotService(ITimeslotRepository repo)
        {
            _repo = repo;
        }

        /*
        public async Task<TimeslotResponseDto> AddTimeslotAsync(TimeslotRequestDto dto)
        {
            TimeOnly dtoStartTime = TimeOnly.Parse(dto.StartTime);
            TimeOnly dtoEndTime = TimeOnly.Parse(dto.EndTime);
            if (dtoEndTime <= dtoStartTime)
            {
                throw new InvalidDataException("End Date can not be less than Start Date");
            }
            if (dto == null)
            {
                throw new Exception("The reqquest object is null");
            }

            var timeslot = new Timeslot
            {
                StartTime = dtoStartTime,
                EndTime = dtoEndTime
            };

            var saved = await _repo.AddAsync(timeslot);
            return new TimeslotResponseDto
            {
                Id = saved.Id,
                StartTime = saved.StartTime,
                EndTime = saved.EndTime
            };
        }

        public async Task DeleteTimeslotAsync(int timeslotId)
        {
            await _repo.DeleteAsync(timeslotId);
        }
        */

        public async Task<IEnumerable<TimeslotResponseDto>> GetAllTimeslotsAsync()
        {
            var timeslots = await _repo.GetAllAsync();
            return timeslots.Select(t => new TimeslotResponseDto
            {
                Id = t.Id,
                StartTime = t.StartTime,
                EndTime = t.EndTime
            });
        }

        public async Task<TimeslotResponseDto?> GetTimeslotByIdAsync(int id)
        {
            var timeslot = await _repo.GetByIdAsync(id);
            if (timeslot == null) return null;

            return new TimeslotResponseDto
            {
                Id = timeslot.Id,
                StartTime = timeslot.StartTime,
                EndTime = timeslot.EndTime
            };
        }

        public async Task<TimeslotResponseDto> AddTimeslotAsync(TimeslotRequestDto dto)
        {
            var newSlot = new Timeslot
            {
                StartTime = TimeOnly.Parse(dto.StartTime),
                EndTime = TimeOnly.Parse(dto.EndTime)
            };

            var created = await _repo.AddAsync(newSlot);

            return new TimeslotResponseDto
            {
                Id = created.Id,
                StartTime = created.StartTime,
                EndTime = created.EndTime
            };
        }

        public async Task<bool> DeleteTimeslotAsync(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            return deleted;
        }
    }
}
