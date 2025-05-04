namespace JobBoard.Dtos
{
    public class ApplicationCreateDto
    {
        public int JobId { get; set; }
        public IFormFile? Resume { get; set; }

        public string? ApplicantName { get; set; }

        public string? ApplicantEmail { get; set; }
        public string? CoverLetter { get; set; }


    }
}

