namespace HostelHelpDesk.Domain.Models
{
    public class Caretaker: User
    {
        public int HostelId { get; set; }
        public Hostel? Hostel { get; set; }
    }
}
