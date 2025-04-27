using Microsoft.EntityFrameworkCore;
using JobBoard.Data;
using JobBoard.Models;
using JobBoard.Dtos;
using JobBoard.Models.Identity;

namespace JobBoard.Services
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application> CreateApplicationAsync(ApplicationCreateDto dto, int candidateId)
        {
            var job = await _context.Jobs.FindAsync(dto.JobId) ?? throw new Exception("Job not found");
            var candidate = await _context.Users.FindAsync(candidateId);
            if (candidate == null || candidate.UserType != "Candidate")
                throw new Exception("Invalid candidate");

            var application = new Application
            {
                Id = dto.JobId,
                ApplicantName = dto.ApplicantName,
                ApplicantEmail = dto.ApplicantEmail,
                Resume = dto.ResumeUrl,
                JobListingId = dto.JobListingId,
                Status = "Pending"

            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<List<Application>> GetApplicationsAsync(int? jobId, int employerId)
        {
            var employer = await _context.Users.FindAsync(employerId);
            if (employer == null || employer.UserType != "Employer")
                throw new Exception("Invalid employer");

            var query = _context.Applications
                .Include(a => a.ApplicantName)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateApplicationAsync(int id, ApplicationUpdateDto ApplicationUpdateDto, int employerId)
        {
            var application = await _context.Applications
                .Include(a => a.ApplicantName)
                .FirstOrDefaultAsync(a => a.Id == id);


            application.Status = ApplicationUpdateDto.Status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}