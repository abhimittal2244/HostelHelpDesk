namespace HostelHelpDesk.Application.DTO
{
    public class HostelUploadResultDto
    {
        public string HostelName { get; set; }
        public string Status { get; set; } // "Success" or "Failed"
        public string Message { get; set; }
    }
}
