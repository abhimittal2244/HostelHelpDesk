namespace HostelHelpDesk.Domain.Models
{
    public class Timeslot
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

    }
}
