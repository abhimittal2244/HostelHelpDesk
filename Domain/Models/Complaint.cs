using System.Text.Json.Serialization;
//using HostelHelpDesk.Shared;
using HostelHelpDesk.Shared.Enums;

namespace HostelHelpDesk.Domain.Models
{
    public class Complaint
    {
        public int Id { get; set; }
        public String ComplaintNo { get; set; }

        // Student navigation and FK
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // Worker is optional
        public int? WorkerId { get; set; }
        public Worker? Worker { get; set; }

        // Caretaker navigation and FK
        public int CaretakerId { get; set; }
        public Caretaker Caretaker { get; set; }

        // Hostel navigation and FK
        public int HostelId { get; set; }
        public Hostel Hostel { get; set; }

        // Room navigation and FK
        public int RoomId { get; set; }
        public Room Room { get; set; }

        // Type and other info
        public int ComplaintTypeId { get; set; }
        public ComplaintType ComplaintType { get; set; }

        // TimeSlot navigation and FK
        public int TimeslotId { get; set; }
        public Timeslot Timeslot { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Completed { get; set; }

        //public string Status { get; set; } = ComplaintStatus.ComplaintStatusDesc.CS101.GetStatus();
        public ComplaintStatus Status { get; set; } = ComplaintStatus.CREATED;
    }
}
