namespace HostelHelpDesk.Application.DTO
{
    public class StudentResponseDto: UserResponseDto
    {
        public int RollNo { get; set; }
        public required string HostelName { get; set; }
        public required string RoomNo { get; set; }
    }
}