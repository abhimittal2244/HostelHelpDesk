using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeslotController : Controller
    {
        private readonly ITimeslotService _timeslotService;

        public TimeslotController(ITimeslotService timeslotService)
        {
            _timeslotService = timeslotService;
        }

        [HttpGet, Authorize(Roles = "Admin,Student")]
        public async Task<ActionResult<IEnumerable<TimeslotResponseDto>>> GetAllTimeslots()
        {
            var timeslots = await _timeslotService.GetAllTimeslotsAsync();
            return Ok(timeslots);
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<TimeslotResponseDto>> GetTimeslotById(int id)
        {
            var timeslot = await _timeslotService.GetTimeslotByIdAsync(id);
            if (timeslot == null)
                return NotFound("Timeslot not found");

            return Ok(timeslot);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<TimeslotResponseDto>> AddTimeslot([FromBody] TimeslotRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _timeslotService.AddTimeslotAsync(dto);
                return CreatedAtAction(nameof(GetTimeslotById), new { id = created.Id }, created);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid time format. Use HH:mm.");
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTimeslot(int id)
        {
            var result = await _timeslotService.DeleteTimeslotAsync(id);
            if (!result)
                return NotFound("Timeslot not found or already deleted");

            return Ok("Timeslot deleted successfully");
        }
    }
}
