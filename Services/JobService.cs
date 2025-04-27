using Microsoft.EntityFrameworkCore;
using JobBoard.Data;
using JobBoard.Models;
using JobBoard.Dtos;

namespace JobBoard.Services
{
    public class JobService
    {
        private readonly ApplicationDbContext _context;

        public JobService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Job>> GetJobsAsync(string? location, int? salaryMin, string? keyword)
        {
            var query = _context.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(location))
                query = query.Where(j => j.Location.Contains(location));

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(j => j.Title.Contains(keyword) || j.Description.Contains(keyword));

            var jobs = await query.Include(j => j.Company).ToListAsync();

            // Lọc salaryMin trong bộ nhớ
            if (salaryMin.HasValue)
            {
                jobs = jobs.Where(j =>
                {
                    try
                    {
                        // Lấy phần đầu tiên của Salary (min) và parse thành int
                        var salaryMinValue = int.Parse(j.Salary.Split('-')[0].Trim());
                        return salaryMinValue >= salaryMin.Value;
                    }
                    catch
                    {
                        return false; // Bỏ qua nếu không parse được
                    }
                }).ToList();
            }

            return jobs;
        }

        public async Task<Job> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.Include(j => j.Company).FirstOrDefaultAsync(j => j.Id == id)
                ?? throw new Exception("Job not found");
        }

        public async Task<Job> CreateJobAsync(JobCreateDto dto, int employerId)
        {
            var employer = await _context.Users.FindAsync(employerId);
            if (employer == null || employer.UserType != "Employer")
                throw new Exception("Invalid employer");

            var job = new Job
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Salary = dto.Salary,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }
    }
}