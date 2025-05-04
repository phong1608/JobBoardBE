using Microsoft.EntityFrameworkCore;
using JobBoard.Data;
using JobBoard.Models;
using JobBoard.Dtos;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using JobBoard.Settings;
using CloudinaryDotNet.Actions;
namespace JobBoard.Services
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;


        public ApplicationService(ApplicationDbContext context,IOptions<CloudinarySetting> config)
        {
            _context = context;
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<Application> CreateApplicationAsync(ApplicationCreateDto dto, int candidateId)
        {
            var job = await _context.Jobs.FindAsync(dto.JobId) ?? throw new Exception("Job not found");
            var candidate = await _context.Users.FindAsync(candidateId);
            if (candidate == null || candidate.UserType != "Candidate")
                throw new Exception("Invalid candidate");
            var resumeUrl = await UploadPdfAsync(dto.Resume);
            var application = new Application
            {
                JobId = dto.JobId,
                ApplicantName = dto.ApplicantName,
                ApplicantEmail = dto.ApplicantEmail,
                ApplicantId = candidateId,
                Resume = resumeUrl,
                CoverLetter=dto.CoverLetter,
                ApplicationDate=DateTime.Now,
                Status = "Pending"

            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }
        public async Task<string?> UploadPdfAsync(IFormFile file)
        {

            using var stream = file.OpenReadStream();

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();

            
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
        public async Task<List<Application>> GetUserApplication(int employerId)
        {
            var employer = await _context.Users.FindAsync(employerId);
            if (employer == null )
                throw new Exception("Invalid employer");

            var query = _context.Applications
                .Where(a => a.ApplicantId==employerId)
                .Include("Job")
                .AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<List<Application>> GetApplicationByEmployer(int employerUd)
        {
            

            var query = _context.Applications
                .Include("Job")
                .Where(a => a.Job.EmployerId == employerUd)
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