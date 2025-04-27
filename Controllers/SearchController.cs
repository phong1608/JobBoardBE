using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JobBoard.Services;

namespace JobBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("jobs")]
        public async Task<IActionResult> SearchJobs([FromQuery] string? keyword, [FromQuery] string? location, [FromQuery] string? skill)
        {
            var jobs = await _searchService.SearchJobsAsync(keyword, location, skill);
            return Ok(jobs);
        }

        //[HttpGet("candidates")]
        //[Authorize(Roles = "Employer")]
        //public async Task<IActionResult> SearchCandidates([FromQuery] string? skill, [FromQuery] int? experience)
        //{
        //    var candidates = await _searchService.SearchCandidatesAsync(skill, experience);
        //    return Ok(candidates);
        //}
    }
}