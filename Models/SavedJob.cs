namespace JobBoard.Models
{
    public class SavedJob
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? JobId { get; set; }
        public JobListing? JobListing { get; set; }
        public DateTime SavedDate { get; set; }

    }
}
