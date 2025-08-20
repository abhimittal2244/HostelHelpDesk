using HostelHelpDesk.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DashboardController(HostelComplaintsDB db) : Controller
    {
        private readonly HostelComplaintsDB _db = db;

        [HttpGet("GetHostelData")]
        public async Task<ActionResult> GetHostelData()
        {
            var hostelData = await _db.Hostels
                .Select(h => new
                {
                    hostelName = h.HostelName,

                    caretaker = _db.Caretakers
                        .Where(c => c.HostelId == h.Id)
                        .Select(c => c.FirstName + " " + c.LastName)
                        .FirstOrDefault(),

                    students = _db.Students
                        .Count(s => s.HostelId == h.Id),

                    complaints = _db.Complaints
                        .Count(c => c.Student.HostelId == h.Id)
                })
                .ToListAsync();

            return Ok(hostelData);
        }

        [HttpGet("ManageHostel")]
        public async Task<ActionResult> ManageHostel()
        {
            var hostelData = await _db.Hostels
                .Select(h => new
                {
                    id  = h.Id,
                    hostelName = h.HostelName,
                    roomNo = _db.Rooms.Count(r => r.HostelId == h.Id)
                })
                .ToListAsync();

            return Ok(hostelData);
        }

        [HttpGet("ManageRoom/{id}")]
        public async Task<ActionResult> ManageRoom(int id)
        {
            var roomData = await _db.Rooms
                .Where(r => r.HostelId == id)
                .Select(r => new
                {
                    id = r.Id,
                    roomNo = r.RoomNo,
                    capacity = _db.Students.Where(s => s.HostelId == id && s.RoomId == r.Id).Count()
                })
                .ToListAsync();

            return Ok(roomData);
        }

        [HttpGet("ManageTimeslot")]
        public async Task<ActionResult> ManageTimeslot()
        {
            var timeslots = await _db.Timeslots
                .Select(t => new
                {
                    timeslots = t.StartTime + " - " + t.EndTime,
                })
                .ToListAsync();

            return Ok(timeslots);
        }
    }
}
