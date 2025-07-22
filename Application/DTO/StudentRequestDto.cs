
using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.Application.DTO
{
    public class StudentRequestDto: UserRequestDto
    {
        public int RollNo { get; set; }
        public int HostelId { get; set; }
        public int RoomId { get; set; }
    }
}