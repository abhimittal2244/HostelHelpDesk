using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using HostelHelpDesk.Models;
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
            return Ok(await _DB.Students.ToListAsync());
        }
        
        [HttpGet("GetAllWorers"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllWorers()
        {
            return Ok(await _DB.Workers.ToListAsync());
        }

        [HttpGet("GetAllCaretakers"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCaretakers()
        {
            return Ok(await _DB.Caretakers.ToListAsync());
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

        /*
         * Copied and Pasted
         * 
        [HttpGet("GetAllHostel"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllHostel()
        {
            var result = await _DB.Hostels.ToListAsync();
            return Ok(result);
        }


        [HttpGet("GetAllRooom"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllRoom()
        {
            var result = await _DB.Rooms.ToListAsync();
            return Ok(result);
        }

        [HttpGet("GetRoom"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetHostelRooms(int HostelId)
        {
            var result = await _DB.Rooms.Where(h => h.Hostel.Id == HostelId).ToListAsync();
            return Ok(result);
        }

        [HttpPost("UploadHostels"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UploadHostels(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is null or empty");

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<HostelRequestDto>(); // Assuming AddHostel matches the CSV structure

                    foreach (var record in records)
                    {
                        var hostel = new Hostel
                        {
                            HostelName = record.HostelName
                            // Set other properties here as needed
                        };

                        await _DB.Hostels.AddAsync(hostel);

                    }

                    await _DB.SaveChangesAsync();
                }

                return Ok("Hostel Data uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddHostel"), Authorize(Roles = "admin")]
        public async Task<IActionResult> AddHostel([FromBody] HostelRequestDto addHostel)
        {
            var request = new Hostel()
            {
                HostelName = addHostel.HostelName
            };
            await _DB.Hostels.AddAsync(request);
            await _DB.SaveChangesAsync();
            return Ok(request);
        }

        [HttpPost("UploadRooms"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UploadRooms(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is null or empty");

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<RoomResponseDto>(); // Assuming AddHostel matches the CSV structure

                    foreach (var record in records)
                    {
                        var hostel = await _DB.Hostels.Where(x => x.HostelName == record.HostelName).ToListAsync();
                        var room = new Room
                        {
                            RoomNo = record.RoomNo,
                            Hostel = hostel.First(),
                            // Set other properties here as needed
                        };

                        await _DB.Rooms.AddAsync(room);

                    }

                    await _DB.SaveChangesAsync();
                }

                return Ok("Rooms Data uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddRoom"), Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRoom([FromBody] RoomRequestDto addRoom)
        {
            var hostel = await _DB.Hostels.FindAsync(addRoom.HostelId);
            if (hostel == null)
            {
                return BadRequest("Hostel not found with ID: " + addRoom.HostelId);
            }
            var request = new Room()
            {
                RoomNo = addRoom.RoomNo,
                Hostel = hostel
            };
            await _DB.Rooms.AddAsync(request);
            await _DB.SaveChangesAsync();
            return Ok(request);
        }
        */

        
    }
}
