using Microsoft.EntityFrameworkCore;
using JobBoard.Data;
using JobBoard.Models;

namespace JobBoard.Services
{
    public class SearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Job>> SearchJobsAsync(string? keyword, string? location, string? skill)
        {
            var query = _context.Jobs.Include(j => j.Company).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(j => j.Title.Contains(keyword) || j.Description.Contains(keyword));

            if (!string.IsNullOrEmpty(location))
                query = query.Where(j => j.Location.Contains(location));

            if (!string.IsNullOrEmpty(skill))
                query = query.Where(j => j.Description.Contains(skill));

            return await query.ToListAsync();
        }
    }
}