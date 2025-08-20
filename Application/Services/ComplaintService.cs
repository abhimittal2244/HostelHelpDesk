using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Persistence.Repository;
using HostelHelpDesk.Shared.Enums;

//using HostelHelpDesk.Shared;
using ComplaintType = HostelHelpDesk.Shared.ComplaintType;

namespace HostelHelpDesk.Application.Services
{
    public class ComplaintService
    {
        private readonly HostelComplaintsDB _db;
        private readonly ComplaintRepository _complaintRepo;
        private readonly ITimeslotRepository _timeslotRepo;
        private readonly ComplaintTypeWorkerTypeRepository _complaintTypeWorkerTypeRepo;
        public ComplaintService(HostelComplaintsDB db, ComplaintRepository complaintRepo, ITimeslotRepository timeslotRepo, ComplaintTypeWorkerTypeRepository complaintTypeWorkerTypeRepo)
        {
            _db = db;
            _complaintRepo = complaintRepo;
            _timeslotRepo = timeslotRepo;
            _complaintTypeWorkerTypeRepo = complaintTypeWorkerTypeRepo;
        }
        //public async Task<ComplaintResponseDto> RaiseComplaintAsync(ComplaintRequestDto addComplaint)
        //{
        //    var stu = await _db.Students.FindAsync(addComplaint.StudentId)
        //              ?? throw new ArgumentException("Student not found");

        //    var caretaker = await _db.Caretakers
        //        .FirstOrDefaultAsync(c => c.Hostel.Id == stu.Hostel.Id)
        //        ?? throw new ArgumentException("No caretaker found for the hostel");

        //    //if (!ComplaintType.GetComplaintType().Contains(addComplaint.Type))
        //    //    throw new ArgumentException("Invalid Complaint Type");

        //    var timeSlot = await _timeslotRepo.GetByIdAsync(addComplaint.TimeSlotId);
        //    var complaintType = await _complaintTypeWorkerTypeRepo.GetComplaintTypeByIdAsync(addComplaint.ComplaintTypeId);

        //    var complaint = new Complaint
        //    {
        //        ComplaintNo = GenerateComplaintNo(), // Ideally a helper/service method
        //        Student = stu,
        //        Caretaker = caretaker,
        //        Timeslot = timeSlot,
        //        Description = addComplaint.Description,
        //        ComplaintType = complaintType,
        //        Hostel = stu.Hostel,
        //        Room = stu.Room
        //    };

        //    await _db.Complaints.AddAsync(complaint);
        //    await _db.SaveChangesAsync();

        //    return new ComplaintResponseDto
        //    {
        //        //Id = complaint.Id,
        //        ComplaintNo = complaint.ComplaintNo,
        //        StudentName = complaint.Student.FirstName + " " + complaint.Student.LastName,
        //        WorkerName = complaint.Worker.FirstName + " " + complaint.Worker.LastName,
        //        caretakerName = complaint.Caretaker.FirstName + " " + complaint.Caretaker.LastName,
        //        hostelName = complaint.Hostel.HostelName,
        //        roomNo = complaint.Room.RoomNo,
        //        type = complaint.ComplaintType.Name,
        //        timeslot = complaint.Timeslot.StartTime + " - " + complaint.Timeslot.EndTime,
        //        description = complaint.Description,
        //        status = complaint.Status.ToString(),

        //        // Add more fields as needed
        //    };
        //}


        public async Task<ComplaintResponseDto> RaiseComplaintAsync(string email, ComplaintRequestDto addComplaint)
        {
            // Validate student
            var stu = await _db.Students
                .Include(s => s.Hostel)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Email == email)
                ?? throw new ArgumentException("Student not found");

            // Find caretaker for the hostel
            var caretaker = await _db.Caretakers
                .Include(c => c.Hostel)
                .FirstOrDefaultAsync(c => c.Hostel.Id == stu.Hostel.Id)
                ?? throw new ArgumentException("No caretaker found for the hostel");

            // Get timeslot and complaint type
            var timeSlot = await _timeslotRepo.GetByIdAsync(addComplaint.TimeSlotId)
                ?? throw new ArgumentException("Invalid timeslot");

            var complaintType = await _complaintTypeWorkerTypeRepo.GetComplaintTypeByIdAsync(addComplaint.ComplaintTypeId)
                ?? throw new ArgumentException("Invalid complaint type");

            // Create new complaint
            var complaint = new Complaint
            {
                ComplaintNo = GenerateComplaintNo(),
                Student = stu,
                Caretaker = caretaker,
                Timeslot = timeSlot,
                Description = addComplaint.Description,
                ComplaintType = complaintType,
                Hostel = stu.Hostel,
                Room = stu.Room,
                Created = DateTime.Now,
                Status = ComplaintStatus.CREATED // assuming you have an enum
            };

            await _db.Complaints.AddAsync(complaint);
            await _db.SaveChangesAsync();

            return new ComplaintResponseDto
            {
                ComplaintNo = complaint.ComplaintNo,
                StudentName = $"{stu.FirstName} {stu.LastName}",
                WorkerName = complaint.Worker != null
                    ? $"{complaint.Worker.FirstName} {complaint.Worker.LastName}"
                    : null,
                caretakerName = $"{caretaker.FirstName} {caretaker.LastName}",
                hostelName = complaint.Hostel.HostelName,
                roomNo = complaint.Room.RoomNo,
                type = complaint.ComplaintType.Name,
                timeslot = $"{timeSlot.StartTime} - {timeSlot.EndTime}",
                description = complaint.Description,
                status = complaint.Status.ToString()
            };
        }

        public static string GenerateComplaintNo()
        {
            return $"CMP-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        public async Task<IEnumerable<ComplaintResponseDto>> GetAllComplaintsAsync()
        {
            var complaints = await _complaintRepo.GetAllAsync();
            return complaints.Select(ToResponseDto);
        }

        public async Task<ComplaintResponseDto> GetComplaintByIdAsync(int complaintId)
        {
            var complaint = await _complaintRepo.GetByIdAsync(complaintId);
            return complaint == null ? null : ToResponseDto(complaint);
        }

        public async Task<IEnumerable<ComplaintResponseDto>> GetByStudentIdAsync(int studentId)
        {
            var complaints = await _complaintRepo.GetByStudentIdAsync(studentId);
            return complaints.Select(ToResponseDto);
        }

        public async Task<IEnumerable<ComplaintResponseDto>> GetByWorkerIdAsync(int workerId)
        {
            var complaints = await _complaintRepo.GetByWorkerIdAsync(workerId);
            return complaints.Select(ToResponseDto);
        }

        public async Task<IEnumerable<ComplaintResponseDto>> GetByCaretakerIdAsync(int caretakerId)
        {
            var complaints = await _complaintRepo.GetByCaretakerIdAsync(caretakerId);
            return complaints.Select(ToResponseDto);
        }

        public async Task<bool> DeleteComplaintAsync(int complaintId)
        {
            return await _complaintRepo.DeleteAsync(complaintId);
        }

        //public async Task<ComplaintResponseDto> AddComplaintAsync(ComplaintRequestDto dto)
        //{
        //    var complaint = await _complaintRepo.AddAsync(dto);
        //    return complaint == null ? null : ToResponseDto(complaint);
        //}


        // Mapping Method
        private ComplaintResponseDto ToResponseDto(Complaint complaint)
        {
            return new ComplaintResponseDto
            {
                //ComplaintId = complaint.Id,
                ComplaintNo = complaint.ComplaintNo,
                type = complaint.ComplaintType.Name,
                description = complaint.Description,
                StudentName = $"{complaint.Student.FirstName} {complaint.Student.LastName}",
                caretakerName = $"{complaint.Caretaker.FirstName} {complaint.Caretaker.LastName}",
                WorkerName = complaint.Worker != null ? $"{complaint.Worker.FirstName} {complaint.Worker.LastName}" : null,
                roomNo = complaint.Room.RoomNo,
                hostelName = complaint.Hostel.HostelName,
                timeslot = $"{complaint.Timeslot.StartTime} - {complaint.Timeslot.EndTime}",
                status = complaint.Status.ToString()
            };
        }
    }
}
