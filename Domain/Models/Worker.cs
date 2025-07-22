namespace HostelHelpDesk.Domain.Models
{
    public class Worker: User
    {
        public ICollection<WorkerType> WorkerSpecialization { get; set; } = new List<WorkerType>();
    }
}
