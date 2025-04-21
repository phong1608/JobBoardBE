using JobBoard.Models;
using JobBoard.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }
        public DbSet<Application> Applications { get; set; } = null!;
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<SavedJob>().ToTable("SavedJobs");
            modelBuilder.Entity<JobListing>().ToTable("JobListings");
            modelBuilder.Entity<Application>().ToTable("Applications");

        }
    }
    
}
