

using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.DTO
{
    public class ComplaintRequestDto
    {
        public int StudentId { get; set; }
        public int ComplaintTypeId { get; set; }
        public int TimeSlotId { get; set; }
        public string? Description { get; set; }
    }
}