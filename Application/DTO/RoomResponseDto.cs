using System.Globalization;

namespace HostelHelpDesk.Application.DTO
{
    public class RoomResponseDto
    {
        public int RoomId { get; set; }
        public string RoomNo { get; set; }
        public int HostelId { get; set; }
        public string HostelName { get; set; }
    }
}