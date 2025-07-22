using System.ComponentModel.DataAnnotations;

namespace HostelHelpDesk.Application.DTO
{
    public class TimeslotRequestDto
    {
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time format")]
        public String StartTime {  get; set; }
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time format")]
        public String EndTime { get; set; }
    }
}