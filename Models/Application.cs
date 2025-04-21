namespace JobBoard.Models
{
    public class Application
    {
        public int Id { get; set; }
        public Guid ApplicantId { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public string Resume { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public int JobListingId { get; set; }
        public JobListing JobListing { get; set; } = null!;
    }
}
