using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JobBoard.Services;
using JobBoard.Dtos;
using System.Security.Claims;

namespace JobBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobService _jobService;

        public JobsController(JobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs([FromQuery] string? location, [FromQuery] int? salaryMin, [FromQuery] string? keyword)
        {
            var jobs = await _jobService.GetJobsAsync(location, salaryMin, keyword);
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            return Ok(job);
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetEmployerJob()
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var job = await _jobService.GetJobByEmployerId(employerId);
            return Ok(job);
        }
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob([FromBody] JobCreateDto dto)
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var job = await _jobService.CreateJobAsync(dto, employerId);
            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job);
        }

        //[HttpPut("{id}")]
        //[Authorize(Roles = "Employer")]

        //public async Task<IActionResult> UpdateJob(int id, [FromBody] JobUpdateDto dto)
        //{
        //    var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //    var updated = await _jobService.UpdateJobAsync(id, dto, employerId);
        //    if (!updated) return NotFound();
        //    return NoContent();
        //}
    }
}