using System.ComponentModel.DataAnnotations;

namespace HostelHelpDesk.Application.DTO
{
    public class TimeslotResponseDto
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
