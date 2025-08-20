using HostelHelpDesk.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;

namespace HostelHelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintTypeWorkerTypeController : Controller
    {
        private readonly ComplaintTypeWorkerTypeService _service;

        public ComplaintTypeWorkerTypeController(ComplaintTypeWorkerTypeService service)
        {
            _service = service;
        }


        [HttpGet("GetAllMappings"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ComplaintTypeWorkerTypeDto>>> GetAllMappings()
        {
            var result = await _service.GetAllMappingsAsync();
            return Ok(result);
        }

        [HttpPost("CreateMapping"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMapping([FromBody] ComplaintTypeWorkerTypeDto dto)
        {
            try
            {
                await _service.CreateMappingAsync(dto);
                return Ok("Mapping created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteMapping"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMapping([FromQuery] int complaintTypeId, [FromQuery] int workerTypeId)
        {
            bool deleted = await _service.DeleteMappingAsync(complaintTypeId, workerTypeId);
            if (!deleted)
                return NotFound("Mapping not found.");

            return Ok("Mapping deleted successfully.");
        }

        [HttpGet("WorkerTypes"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllWorkerTypes()
        {
            var workerTypes = await _service.GetAllWorkerTypesAsync();
            return Ok(workerTypes);
        }

        [HttpGet("ComplaintTypes"), Authorize(Roles = "Admin,Student")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllComplaintTypes()
        {
            var complaintTypes = await _service.GetAllComplaintTypesAsync();
            return Ok(complaintTypes);
        }
    }
}
