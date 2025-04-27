namespace JobBoard.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public int CandidateId { get; set; }
        public User Candidate { get; set; }
        public string ResumeUrl { get; set; }
        public string? CoverLetter { get; set; }
        public string Status { get; set; } // "Pending", "Reviewed", "Accepted", "Rejected"
        public DateTime AppliedAt { get; set; }
    }
}