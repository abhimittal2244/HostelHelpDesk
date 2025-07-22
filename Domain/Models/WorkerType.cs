namespace HostelHelpDesk.Domain.Models
{
    public class WorkerType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ComplaintTypeId { get; set; }
        public ComplaintType ComplaintType { get; set; }

    }
}
