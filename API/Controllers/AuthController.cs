using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Services;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private readonly HostelComplaintsDB _DB;
        private readonly JwtServices _auth;
        public AuthController(HostelComplaintsDB DB, JwtServices auth)
        {
            _DB = DB;
            _auth = auth;
        }

        //[HttpGet("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        //{
        //    return _auth.login(dto);
        //}

        //var email = User.FindFirstValue(ClaimTypes.Email);

        [HttpPost("UploadStudents"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadStudents(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is null or empty");

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<StudentResponseDto>(); // Assuming AddHostel matches the CSV structure

                    foreach (var record in records)
                    {
                        var hostel = await _DB.Hostels.Where(x => x.HostelName == record.HostelName).ToListAsync();
                        var room = await _DB.Rooms.Where(x => x.RoomNo == record.RoomNo).ToListAsync();
                        _auth.CreatePasswordHash(record.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        var stu = new Student
                        {
                            FirstName = record.FirstName,
                            LastName = record.LastName,
                            Email = record.Email,
                            PhoneNumber = record.PhoneNumber,
                            RollNo = record.RollNo,
                            Hostel = hostel.First(),
                            Room = room.First(),
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt
                        };

                        await _DB.Students.AddAsync(stu);

                    }

                    await _DB.SaveChangesAsync();
                }

                return Ok("Students Data uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddStudent"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<StudentResponseDto>> AddStudent([FromBody] StudentRequestDto addStudent)
        {
            _auth.CreatePasswordHash(addStudent.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var hostel = await _DB.Hostels.FindAsync(addStudent.HostelId);
            var room = await _DB.Rooms.FindAsync(addStudent.RoomId);
            if (hostel == null)
            {
                return BadRequest("hostel not found");
            }
            if (room == null)
            {
                return BadRequest("Room not found");
            }
            var request = new Student()
            {
                FirstName = addStudent.FirstName,
                LastName = addStudent.LastName,
                Email = addStudent.Email,
                PhoneNumber = addStudent.PhoneNumber,
                RollNo = addStudent.RollNo,
                Hostel = hostel,
                Room = room,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = Shared.Enums.Role.Student

            };
            await _DB.Students.AddAsync(request);
            await _DB.SaveChangesAsync();
            var response = new StudentResponseDto
            {
                Id = request.Id,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                RollNo = request.RollNo,
                Password = addStudent.Password,
                HostelName = request.Hostel.HostelName,
                RoomNo = request.Room.RoomNo
            };
            return Ok(response);
        }

        [HttpPost("UploadWorker"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadWorker(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is null or empty");

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<WorkerRequestDto>(); // Assuming AddHostel matches the CSV structure

                    foreach (var record in records)
                    {
                        _auth.CreatePasswordHash(record.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        var specializationEntities = await _DB.WorkerTypes
                            .Where(wt => record.WorkerSpecialization.Contains(wt.Id))
                            .ToListAsync();
                        var worker = new Worker
                        {
                            Email = record.Email,
                            FirstName = record.FirstName,
                            LastName = record.LastName,
                            PhoneNumber = record.PhoneNumber,
                            WorkerSpecialization = specializationEntities,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt
                        };

                        await _DB.Workers.AddAsync(worker);

                    }

                    await _DB.SaveChangesAsync();
                }

                return Ok("Worker Data uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddWorker"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<WorkerResponseDto>> AddWorker([FromBody] WorkerRequestDto addWorker)
        {
            // Generate password hash and salt
            _auth.CreatePasswordHash(addWorker.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Fetch specialization entities by name
            var specializationEntities = await _DB.WorkerTypes
                .Where(wt => addWorker.WorkerSpecialization.Contains(wt.Id))
                .ToListAsync();

            if (specializationEntities.Count != addWorker.WorkerSpecialization.Count)
            {
                return BadRequest("One or more specialization names are invalid.");
            }

            // Create Worker entity
            var worker = new Worker
            {
                Email = addWorker.Email,
                FirstName = addWorker.FirstName,
                LastName = addWorker.LastName,
                PhoneNumber = addWorker.PhoneNumber,
                WorkerSpecialization = specializationEntities,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            // Save to DB
            await _DB.Workers.AddAsync(worker);
            await _DB.SaveChangesAsync();

            // Prepare response
            var response = new WorkerResponseDto
            {
                Id = worker.Id,
                Email = worker.Email,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                PhoneNumber = worker.PhoneNumber,
                Password = addWorker.Password,
                WorkerSpecialization = specializationEntities.Select(w => w.Name).ToList()
            };

            return Ok(response);
        }

        [HttpPost("UploadCaretaker"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadCaretaker(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is null or empty");

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<CaretakerResponseDto>(); // Assuming AddHostel matches the CSV structure

                    foreach (var record in records)
                    {
                        var hostel = await _DB.Hostels.Where(x => x.HostelName == record.HostelName).ToListAsync();
                        _auth.CreatePasswordHash(record.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        var caretaker = new Caretaker
                        {
                            FirstName = record.FirstName,
                            LastName = record.LastName,
                            Email = record.Email,
                            PhoneNumber = record.PhoneNumber,
                            Hostel = hostel.First(),
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt
                        };

                        await _DB.Caretakers.AddAsync(caretaker);

                    }

                    await _DB.SaveChangesAsync();
                }

                return Ok("Caretakers Data uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddCaretaker"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<CaretakerResponseDto>> AddCaretaker([FromBody] CaretakerRequestDto addCaretaker)
        {
            var hostel = await _DB.Hostels.FindAsync(addCaretaker.HostelId);
            if (hostel == null)
            {
                return BadRequest("Hostel not found");
            }
            _auth.CreatePasswordHash(addCaretaker.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var request = new Caretaker()
            {
                Email = addCaretaker.Email,
                FirstName = addCaretaker.FirstName,
                LastName = addCaretaker.LastName,
                PhoneNumber = addCaretaker.PhoneNumber,
                Hostel = hostel,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _DB.Caretakers.AddAsync(request);
            await _DB.SaveChangesAsync();
            var response = new CaretakerResponseDto
            {
                Id = request.Id,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Password = addCaretaker.Password,
                HostelName = request.Hostel.HostelName
            };
            return Ok(request);
        }

        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody] AdminRequestDto dto)
        {
            _auth.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var request = new Admin()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = dto.PhoneNumber
            };
            await _DB.Admins.AddAsync(request);
            await _DB.SaveChangesAsync();
            return Ok(request);
        }

        [HttpPost("StudentLogin")]
        public async Task<IActionResult> StudentLogin([FromBody] LoginDTO dto)
        {
            User? student = await _DB.Students.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if(student == null)
            {
                return BadRequest("User Not Found");
            }
            if(!_auth.VerifyPassowrd(dto.Password, student.PasswordHash, student.PasswordSalt))
            {
                return BadRequest("Incorrect Password");
            }
            string token = _auth.CreateToken(student.Email, Shared.Enums.Role.Student.ToString());
            return Ok(new { token });
        }

        [HttpPost("WorkerLogin")]
        public async Task<IActionResult> WorkerLogin([FromBody] LoginDTO dto)
        {
            User? worker = await _DB.Workers.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (worker == null)
            {
                return BadRequest("User Not Found");
            }
            if (!_auth.VerifyPassowrd(dto.Password, worker.PasswordHash, worker.PasswordSalt))
            {
                return BadRequest("Incorrect Password");
            }
            string token = _auth.CreateToken(worker.Email, Shared.Enums.Role.Worker.ToString());
            return Ok(new { token });
        }

        [HttpPost("CaretakerLogin")]
        public async Task<IActionResult> CaretakerLogin([FromBody] LoginDTO dto)
        {
            User? caretaker = await _DB.Caretakers.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (caretaker == null)
            {
                return BadRequest("User Not Found");
            }
            if (!_auth.VerifyPassowrd(dto.Password, caretaker.PasswordHash, caretaker.PasswordSalt))
            {
                return BadRequest("Incorrect Password");
            }
            string token = _auth.CreateToken(caretaker.Email, Shared.Enums.Role.Caretaker.ToString());
            return Ok(new { token });
        }

        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginDTO dto)
        {
            Admin? admin = await _DB.Admins.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (admin == null)
            {
                return BadRequest("Invaild Crediantials");
            }
            if (!_auth.VerifyPassowrd(dto.Password, admin.PasswordHash, admin.PasswordSalt))
            {
                return BadRequest("Invaid Crediantials");
            }
            string token = _auth.CreateToken(admin.Email, Shared.Enums.Role.Admin.ToString());
            /*if(_auth.IsTokenValid(token)) 
            { 
                return Ok(token);
            }
            return BadRequest("Token Expired");*/
            return Ok(new { token });
        }
    }
}
