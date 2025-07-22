namespace HostelHelpDesk.Domain.Models
{
    public class Room
    {
        public int Id { get; set; }
        public required string RoomNo { get; set; }
        public int HostelId { get; set; }
        public Hostel Hostel { get; set; }
    }
}
