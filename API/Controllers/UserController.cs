using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Services;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Domain.Models;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private readonly HostelComplaintsDB _DB;
        private readonly JwtServices _auth;

        public UserController(HostelComplaintsDB DB, JwtServices auth)
        {
            _DB = DB;
            _auth = auth;
        }

        [HttpGet("GetAllStudents"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _DB.Students.Include(h => h.Hostel).Include(r => r.Room)
                .Select(s => new
                {
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    RollNo = s.RollNo,
                    RoomNo = s.Room.RoomNo,
                    HostelName = s.Hostel.HostelName

                }).ToListAsync();

            return Ok(students);
        }
        
        [HttpGet("GetAllWorers"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllWorers()
        {
            var workers = await _DB.Workers
                .Select(s => new
                {
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    WorkerSpecialization = s.WorkerSpecialization.ToArray(),

                }).ToListAsync();

            return Ok(workers);
        }

        [HttpGet("GetAllCaretakers"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCaretakers()
        {
            var caretakers = await _DB.Caretakers.Include(h => h.Hostel)
                .Select(s => new
                {
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    HostelName = s.Hostel.HostelName

                }).ToListAsync();

            return Ok(caretakers);
        }
        
        [HttpGet("GetStudent"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStudent(int rollNo)
        {
            var result = await _DB.Students.FirstOrDefaultAsync(x => x.RollNo == rollNo);
            return Ok(result);
        }

        [HttpGet("GetWorker"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWorker(string phoneNo)
        {
            var result = await _DB.Workers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNo);
            return Ok(result);
        }

        [HttpGet("GetCaretaker"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCaretaker(string phoneNo)
        {
            var result = await _DB.Caretakers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNo);
            return Ok(result);
        }

    }
}
