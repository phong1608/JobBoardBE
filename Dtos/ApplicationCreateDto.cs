namespace JobBoard.Dtos
{
    public class ApplicationCreateDto
    {
        public int JobId { get; set; }
        public string ResumeUrl { get; set; }

        public string ApplicantName { get; set; }

        public string ApplicantEmail { get; set; }

        public int JobListingId { get; set; }

    }
}

