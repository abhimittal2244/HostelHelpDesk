using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Services;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Shared.Enums;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : Controller
    {
        private readonly HostelComplaintsDB _DB;
        private readonly ComplaintService _complaintService;
        public ComplaintsController(HostelComplaintsDB DB, ComplaintService complaintService)
        {
            _DB = DB;
            _complaintService = complaintService;
        }


        [HttpGet("GetAllComplaint")]
        public async Task<IActionResult> GetAllComplaint()
        {
            var complaints = await _DB.Complaints
                .Include(c => c.Caretaker)
                .Include(c => c.Student)
                .Include(c => c.Worker)
                .Include(c => c.Hostel)
                .Include(c => c.Room)
                .Include(c => c.ComplaintType)
                .Include(c => c.Timeslot)
                .ToListAsync();
            return Ok(complaints);
        }

        //[HttpGet("GetAllStudentComplaint")]
        //public async Task<IActionResult> GetAllStudentComplaint(string email)
        //{
        //    var stu = await _DB.Students
        //        .Include(s => s.Hostel)
        //        .Include(s => s.Room)
        //        .FirstOrDefaultAsync(x => x.Email == email);
        //    var complaints = await _DB.Complaints
        //        .Include(s => s.Student)
        //        .Include(s => s.Worker)
        //        .Include(s => s.Caretaker)
        //        .Include(s => s.Hostel)
        //        .Include(s => s.Room)
        //        .Include(s => s.ComplaintType)
        //        .Include(s => s.Timeslot)
        //        .Where(s => s.Student.Id == stu.Id).ToListAsync();
        //    List<ComplaintResponseDto> complaintDTOs = new List<ComplaintResponseDto>();
        //    foreach (var complaint in complaints)
        //    {
        //        var student = await _DB.Students.FindAsync(complaint.Student.Id);
        //        var worker = await _DB.Workers.FindAsync(complaint.Worker.Id);
        //        string workerName = "None";
        //        if (worker != null)
        //        {
        //            workerName = $"{worker.FirstName} {worker.LastName}";
        //        }
        //        var caretaker = await _DB.Caretakers.FindAsync(complaint.Caretaker.Id);
        //        var hostel = await _DB.Hostels.FindAsync(complaint.Hostel.Id);
        //        var room = await _DB.Rooms.FindAsync(student.RoomId);
        //        var timeslot = await _DB.Timeslots.FindAsync(complaint.Timeslot.Id);
        //        var complaintType = await _DB.ComplaintTypes.FindAsync(complaint.ComplaintTypeId);

        //        var complaintDTO = new ComplaintResponseDto()
        //        {
        //            ComplaintNo = complaint.ComplaintNo,
        //            StudentName = student.FirstName,
        //            WorkerName = workerName,
        //            caretakerName = caretaker.FirstName,
        //            hostelName = hostel.HostelName,
        //            roomNo = room.RoomNo,
        //            type = complaintType.Name.ToString(),
        //            description = complaint.Description,
        //            timeslot = timeslot.StartTime.ToString("HH:mm"),
        //            status = complaint.Status.ToString()
        //        };
        //        complaintDTOs.Add(complaintDTO);
        //    }

        //    return Ok(complaintDTOs);
        //}

        [Authorize]
        [HttpGet("GetUserComplaints")]
        public async Task<IActionResult> GetUserComplaints()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token");
            if(string.IsNullOrEmpty(role))
                return Unauthorized("Role not found in token");

            try
            {
                var complaints = new List<Complaint>();
                if(role == Role.Student.ToString())
                {
                    var stu = await _DB.Students
                        .Include(s => s.Hostel)
                        .Include(s => s.Room)
                        .FirstOrDefaultAsync(x => x.Email == email);


                    if (stu == null)
                        return NotFound("Student not found");

                    complaints = await _DB.Complaints
                        .Include(c => c.Student)
                        .Include(c => c.Worker)
                        .Include(c => c.Caretaker)
                        .Include(c => c.Hostel)
                        .Include(c => c.Room)
                        .Include(c => c.ComplaintType)
                        .Include(c => c.Timeslot)
                        .Where(c => c.Student.Id == stu.Id)
                        .ToListAsync();
                }
                else if (role == Role.Caretaker.ToString())
                {
                    var caretaker = await _DB.Caretakers
                        .Include(s => s.Hostel)
                        .FirstOrDefaultAsync(x => x.Email == email);


                    if (caretaker == null)
                        return NotFound("Caretaker not found");

                    complaints = await _DB.Complaints
                        .Include(c => c.Student)
                        .Include(c => c.Worker)
                        .Include(c => c.Caretaker)
                        .Include(c => c.Hostel)
                        .Include(c => c.Room)
                        .Include(c => c.ComplaintType)
                        .Include(c => c.Timeslot)
                        .Where(c => c.Caretaker.Id == caretaker.Id)
                        .ToListAsync();
                }
                else if (role == Role.Worker.ToString())
                {
                    var worker = await _DB.Workers
                        .FirstOrDefaultAsync(x => x.Email == email);


                    if (worker == null)
                        return NotFound("Worker not found");

                    complaints = await _DB.Complaints
                        .Include(c => c.Student)
                        .Include(c => c.Worker)
                        .Include(c => c.Caretaker)
                        .Include(c => c.Hostel)
                        .Include(c => c.Room)
                        .Include(c => c.ComplaintType)
                        .Include(c => c.Timeslot)
                        .Where(c => c.Worker.Id == worker.Id)
                        .ToListAsync();
                }
                else if (role == Role.Admin.ToString())
                {
                    var admin = await _DB.Admins
                        .FirstOrDefaultAsync(x => x.Email == email);


                    if (admin == null)
                        return NotFound("admin not found");

                    complaints = await _DB.Complaints
                        .Include(c => c.Student)
                        .Include(c => c.Worker)
                        .Include(c => c.Caretaker)
                        .Include(c => c.Hostel)
                        .Include(c => c.Room)
                        .Include(c => c.ComplaintType)
                        .Include(c => c.Timeslot)
                        .ToListAsync();
                }
                else
                {
                    return BadRequest("Invalid Role given: " + role);
                }


                    var complaintDTOs = complaints.Select(complaint =>
                        {
                            string workerName = complaint.Worker != null
                                ? $"{complaint.Worker.FirstName} {complaint.Worker.LastName}"
                                : "None";

                            return new ComplaintResponseDto
                            {
                                ComplaintNo = complaint.ComplaintNo,
                                StudentName = $"{complaint.Student?.FirstName ?? "N/A"} {complaint.Student?.LastName ?? ""}",
                                WorkerName = workerName,
                                caretakerName = complaint.Caretaker?.FirstName ?? "N/A",
                                hostelName = complaint.Hostel?.HostelName ?? "N/A",
                                roomNo = complaint.Room?.RoomNo ?? "N/A",
                                type = complaint.ComplaintType?.Name ?? "N/A",
                                description = complaint.Description,
                                timeslot = complaint.Timeslot != null
                                    ? $"{complaint.Timeslot.StartTime:HH:mm} - {complaint.Timeslot.EndTime:HH:mm}"
                                    : "N/A",
                                status = complaint.Status.ToString()
                            };
                        }).ToList();

                return Ok(complaintDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("GetAllStudentComplaint")]
        public async Task<IActionResult> GetAllStudentComplaint()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token");

            try
            {
                var stu = await _DB.Students
                    .Include(s => s.Hostel)
                    .Include(s => s.Room)
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (stu == null)
                    return NotFound("Student not found");

                var complaints = await _DB.Complaints
                    .Include(c => c.Student)
                    .Include(c => c.Worker)
                    .Include(c => c.Caretaker)
                    .Include(c => c.Hostel)
                    .Include(c => c.Room)
                    .Include(c => c.ComplaintType)
                    .Include(c => c.Timeslot)
                    .Where(c => c.Student.Id == stu.Id)
                    .ToListAsync();

                var complaintDTOs = complaints.Select(complaint =>
                {
                    string workerName = complaint.Worker != null
                        ? $"{complaint.Worker.FirstName} {complaint.Worker.LastName}"
                        : "None";

                    return new ComplaintResponseDto
                    {
                        ComplaintNo = complaint.ComplaintNo,
                        StudentName = $"{complaint.Student?.FirstName ?? "N/A"} {complaint.Student?.LastName ?? ""}",
                        WorkerName = workerName,
                        caretakerName = complaint.Caretaker?.FirstName ?? "N/A",
                        hostelName = complaint.Hostel?.HostelName ?? "N/A",
                        roomNo = complaint.Room?.RoomNo ?? "N/A",
                        type = complaint.ComplaintType?.Name ?? "N/A",
                        description = complaint.Description,
                        timeslot = complaint.Timeslot != null
                            ? $"{complaint.Timeslot.StartTime:HH:mm} - {complaint.Timeslot.EndTime:HH:mm}"
                            : "N/A",
                        status = complaint.Status.ToString()
                    };
                }).ToList();

                return Ok(complaintDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("GetAllCaretakerCompaint")]
        public async Task<ActionResult<CaretakerComplaintsDto[]>> GetAllCaretakerCompaint()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token");
            var caretaker = await _DB.Caretakers.FirstOrDefaultAsync(s => s.Email == email);
                        var complaints = await _DB.Complaints
                .Where(a => a.CaretakerId == caretaker.Id)
                .Include(c => c.Worker)
                .Include(c => c.Student)
                .Include(c => c.Room)
                .Include(c => c.Timeslot)
                .ToListAsync();

            var getCaretakerComplaints = complaints.Select(c => new CaretakerComplaintsDto
            {
                complaintNo = c.ComplaintNo,
                worker = c.Worker?.FirstName,
                studentName = c.Student?.FirstName,
                roomNo = c.Room?.RoomNo,
                description = c.Description,
                status = c.Status.ToString(),
                timeslot = $"{c.Timeslot?.StartTime.ToString("HH:mm")} - {c.Timeslot?.EndTime.ToString("HH:mm")}"
            }).ToList();


            return Ok(getCaretakerComplaints);
        }

        [Authorize]
        [HttpGet("GetAllWorkerCompaint")]
        public async Task<ActionResult<WorkerComplaintsDto[]>> GetAllWorkerCompaint()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token");
            var worker = await _DB.Workers.FirstOrDefaultAsync(s => s.Email == email);
            var complaints = await _DB.Complaints
                .Where(a => a.WorkerId == worker.Id)
                .Include(c => c.Worker)
                .Include(c => c.Student)
                .Include(c => c.Hostel)
                .Include(c => c.Room)
                .Include(c => c.Timeslot)
                .ToListAsync();

            var getWorkerComplaints = complaints.Select(c => new WorkerComplaintsDto
            {
                complaintNo = c.ComplaintNo,
                studentName = c.Student?.FirstName,
                hostelName = c.Hostel?.HostelName,
                roomNo = c.Room?.RoomNo,
                description = c.Description,
                status = c.Status.ToString(),
                timeslot = $"{c.Timeslot?.StartTime.ToString("HH:mm")} - {c.Timeslot?.EndTime.ToString("HH:mm")}"
            }).ToList();
            //var complaints = await _DB.Complaints.Where(a => a.Worker.Id == worker.Id).ToListAsync();
            //List<WorkerComplaintsDto> getWorkerComplaints = new List<WorkerComplaintsDto>();
            //foreach (var complaint in complaints)
            //{
            //    var hostel = await _DB.Hostels.FirstOrDefaultAsync(c => c.Id == complaint.Hostel.Id);
            //    var stu = await _DB.Students.FirstOrDefaultAsync(c => c.Id == complaint.Student.Id);
            //    var room = await _DB.Rooms.FirstOrDefaultAsync(x => x.Id == complaint.Room.Id);
            //    var timeslot = await _DB.Timeslots.FindAsync(complaint.Timeslot.Id);
            //    var comp = new WorkerComplaintsDto()
            //    {
            //        //complaintNo = complaint.ComplaintNo,
            //        hostelName = hostel.HostelName,
            //        studentName = stu.FirstName,
            //        roomNo = room.RoomNo,
            //        description = complaint.Description,
            //        status = complaint.Status.ToString(),
            //        timeslot = timeslot.StartTime.ToString("HH:mm")
            //    };
            //    getWorkerComplaints.Add(comp);
            //}
            return Ok(getWorkerComplaints);
        }

        [Authorize]
        [HttpPost("CreateComplaint")]
        public async Task<ActionResult<ComplaintResponseDto>> RaiseComplaintAsync(ComplaintRequestDto addComplaint)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token");
            var created = await _complaintService.RaiseComplaintAsync(email, addComplaint);
            return Ok(created);
        }

        //[HttpPost("RaiseComplaint"), Authorize(Roles = "student")]
        //public async Task<IActionResult> RaiseComplaint(ComplaintRequestDto addComplaint)
        //{
        //    try
        //    {
        //        var stu = await _DB.Students.FindAsync(addComplaint.StudentId);
        //        if (stu == null)
        //        {
        //            throw new ArgumentException("Student not found");
        //        }

        //        var caretaker = await _DB.Caretakers.FirstOrDefaultAsync(c => c.Hostel.Id == stu.Hostel.Id);
        //        if (caretaker == null)
        //        {
        //            throw new ArgumentException("No caretaker found for the hostel");
        //        }

        //        foreach(string type in ComplaintType.GetComplaintType())
        //        {
        //            if (type == addComplaint.Type)
        //            {
        //                var timeSlot = await _DB.Timeslots.FindAsync(addComplaint.TimeSlotId);
        //                var request = new Complaint()
        //                {
        //                    ComplaintNo = new int(),
        //                    Student = stu,
        //                    //Worker = Guid.NewGuid(),
        //                    Caretaker = caretaker,
        //                    Timeslot = timeSlot,
        //                    Description = addComplaint.Description,
        //                    Type = GetWorkerType(addComplaint.Type),
        //                    Hostel = stu.Hostel,
        //                    Room = stu.Room
        //                };
        //                await _DB.Complaints.AddAsync(request);
        //                await _DB.SaveChangesAsync();
        //                return Ok(request);
        //            }
        //        }
        //        return BadRequest("Invadid Complaint Type");               
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        // Handle specific exceptions
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        // Handle other unexpected exceptions
        //        return StatusCode(500, "An error occurred while processing your request");
        //    }
        //}

        [HttpPut("UpdateComplaint"), Authorize(Roles = "Worker,Student")]
        public async Task<IActionResult> UpdateComplaint([FromQuery]int complaintId, bool status)
        {
            Complaint? complaint = await _DB.Complaints.FirstOrDefaultAsync(c => c.Id == complaintId);

            if (complaint == null)
            {
                return NotFound();
            }
            if (status)
            {
                complaint.Status = ComplaintStatus.RESOLVED;
            }
            else
            {
                complaint.Status = ComplaintStatus.PENDING;
            }

            await _DB.SaveChangesAsync();

            return Ok();
        }

        //[HttpGet("GetTimeSlots")]
        //public async Task<IActionResult> GetTimeSlots()
        //{
        //    var timeslots = await _DB.Timeslots.ToListAsync();
        //    return Ok(timeslots);
        //}

        //[HttpPost("AddTimeSlot")]
        //public async Task<IActionResult> AddTimeSlot(TimeslotRequestDto dto)
        //{
        //    try
        //    {
        //        var timeslot = new Timeslot()
        //        {
        //            StartTime = TimeOnly.Parse(dto.StartTime),
        //            EndTime = TimeOnly.Parse(dto.EndTime)
        //        };
        //        await _DB.Timeslots.AddAsync(timeslot);
        //        await _DB.SaveChangesAsync();
        //        return Ok(timeslot);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        return StatusCode(500, "An error occurred while processing the request.");
        //    }
        //}
    }
}