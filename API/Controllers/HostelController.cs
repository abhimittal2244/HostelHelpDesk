using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Application.Services;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class HostelController : Controller
    {
        private readonly HostelService _hostelService;
        private readonly HostelComplaintsDB _DB;

        public HostelController(HostelService hostelService, HostelComplaintsDB DB)
        {
            _hostelService = hostelService;
            _DB = DB;
        }

        [HttpPost("add"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<HostelResponseDto>> AddHostel1([FromBody] HostelRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.HostelName))
            {
                return BadRequest("Invalid hostel data.");
            }

            var isExist = await _hostelService.IsHostelExistAsync(request.HostelName);
            if (isExist)
            {
                return Conflict("Hostel with the same name already exists.");
            }

            var result = await _hostelService.AddHostelAsync(request);
            return Ok(result);
        }

        [HttpPost("UploadHostels"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadHostels(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is null or empty");

            using var reader = new StreamReader(file.OpenReadStream());
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<HostelRequestDto>().ToList();

            var resultRecords = new List<HostelUploadResultDto>();

            foreach (var record in records)
            {
                var result = new HostelUploadResultDto
                {
                    HostelName = record.HostelName
                };

                try
                {
                    var isExist = await _hostelService.IsHostelExistAsync(record.HostelName);

                    if (isExist)
                    {
                        result.Status = "Failed";
                        result.Message = "Hostel already exists";
                    }
                    else
                    {
                        await _hostelService.AddHostelAsync(record);
                        result.Status = "Success";
                        result.Message = "Hostel added successfully";
                    }
                }
                catch (Exception ex)
                {
                    result.Status = "Failed";
                    result.Message = $"Error: {ex.Message}";
                }

                resultRecords.Add(result);
            }

            // Convert resultRecords to downloadable CSV
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(resultRecords);
            writer.Flush();
            memoryStream.Position = 0;

            return File(memoryStream.ToArray(), "text/csv", "UploadResult.csv");
        }

        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<HostelResponseDto>>> GetAllHostels()
        {
            var hostels = await _hostelService.GetAllHostelAsync();
            if (!hostels.Any())
                return NotFound("No hostels found.");

            return Ok(hostels);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHostel(int id)
        {
            try
            {
                await _hostelService.DeleteHostelAsync(id);
                return Ok("Hostel deleted successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
