using HostelHelpDesk.Shared.Enums;

namespace HostelHelpDesk.Application.DTO
{
    public class ComplaintResponseDto
    {
        public string ComplaintNo { get; set; }
        public string StudentName { get; set; }
        public string WorkerName { get; set;}
        public string caretakerName { get; set;}
        public string hostelName { get; set;}
        public string roomNo { get; set;}
        public string type { get; set;}
        public string description { get; set;}
        public string timeslot { get; set;}
        public string status { get; set;}
    }
}