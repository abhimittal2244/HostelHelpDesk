using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Application.Services;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RoomController(RoomService roomService) : Controller
    {
        private readonly RoomService _roomService = roomService;

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRooms()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching rooms: {ex.Message}");
            }
        }

        [HttpGet("/hostel/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllHostelRooms(int id)
        {
            try
            {
                var rooms = await _roomService.GetRoomsByHostelIdAsync(id);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching rooms: {ex.Message}");
            }
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomResponseDto>> AddRoom([FromBody] RoomRequestDto dto)
        {
            try
            {
                var created = await _roomService.AddRoomAsync(dto);
                return CreatedAtAction(nameof(GetRoomById), new { id = created.RoomId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding room: {ex.Message}");
            }
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomResponseDto>> GetRoomById(int id)
        {
            try
            {
                var room = await _roomService.GetRoomByIdAsync(id);
                if (room == null)
                    return NotFound("Room not found.");
                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching room: {ex.Message}");
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                var deleted = await _roomService.DeleteRoomAsync(id);
                if (!deleted)
                    return NotFound("Room not found or already deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting room: {ex.Message}");
            }
        }

        [HttpPost("upload"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadRooms(IFormFile file)
        {
            try
            {
                var result = await _roomService.UploadRoomsAsync(file);
                return File(result, "text/csv", "UploadResult.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while uploading rooms: {ex.Message}");
            }
        }
    }
}
