namespace HostelHelpDesk.Domain.Models
{
    public class Student: User
    {
        public int RollNo { get; set; }
        public int HostelId { get; set; }
        public Hostel? Hostel { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
