using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JobBoard.Services;
using JobBoard.Dtos;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public ApplicationsController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]

        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> CreateApplication([FromForm] ApplicationCreateDto dto)
        {
            var candidateId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var application = await _applicationService.CreateApplicationAsync(dto, candidateId);
            return CreatedAtAction(nameof(GetApplications), new { id = application.Id }, application);
        }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetApplications([FromQuery] int? jobId)
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var applications = await _applicationService.GetApplicationsAsync(jobId, employerId);
            return Ok(applications);
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUserApplication([FromQuery] int? jobId)
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var applications = await _applicationService.GetUserApplication(employerId);
            return Ok(applications);
        }
        [HttpGet("employer")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetApplicationByEmployer([FromQuery] int? jobId)
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var applications = await _applicationService.GetApplicationByEmployer(employerId);
            return Ok(applications);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody] ApplicationUpdateDto dto)
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var updated = await _applicationService.UpdateApplicationAsync(id, dto, employerId);
            if (!updated) return NotFound();
            return NoContent();
        }

    }
}