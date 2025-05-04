namespace JobBoard.Dtos
{
    public class JobCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Salary { get; set; }
        public int EmployerId { get; set; }
    }
}