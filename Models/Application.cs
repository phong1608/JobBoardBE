namespace JobBoard.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public User? Applicant { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public string Resume { get; set; } = string.Empty;
        public string CoverLetter { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;
        public required string Status { get; set; }
    }
}
