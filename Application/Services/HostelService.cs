using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Persistence.Repository;

namespace HostelHelpDesk.Application.Services
{
    public class HostelService
    {
        private readonly HostelRepository _hostelRepository;
        private readonly HostelComplaintsDB _db;

        public HostelService(HostelRepository hostelRepository, HostelComplaintsDB db)
        {
            _hostelRepository = hostelRepository;
            _db = db;
        }


        public async Task<HostelResponseDto> AddHostelAsync(HostelRequestDto dto)
        {
            if (await _hostelRepository.IsExistAsync(dto.HostelName))
                throw new ArgumentException("Hostel with this name already exists");

            var hostel = new Hostel { HostelName = dto.HostelName };
            await _hostelRepository.AddAsync(hostel);

            return new HostelResponseDto
            {
                Id = hostel.Id,
                HostelName = hostel.HostelName
            };
        }

        public async Task<bool> DeleteHostelAsync(int id)
        {
            var hostel = await _hostelRepository.FindAsync(id);
            if (hostel == null)
            {
                throw new ArgumentException($"No hostel found with ID {id}");
            }

            await _hostelRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<HostelResponseDto>> GetAllHostelAsync()
        {
            var hostels = await _hostelRepository.GetAllAsync();

            return hostels.Select(h => new HostelResponseDto
            {
                Id = h.Id,
                HostelName = h.HostelName
            }).ToList();
        }

        public async Task<bool> IsHostelExistAsync(string hostelName)
        {
            var hostels = await _hostelRepository.FindAllByNameAsync(hostelName);
            return hostels != null && hostels.Any();
        }


    }
}
