namespace HostelHelpDesk.Application.DTO
{
    public class WorkerRequestDto : UserRequestDto
    {
        public required List<int> WorkerSpecialization { get; set; }
    }
}