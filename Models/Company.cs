using JobBoard.Models;

namespace JobBoard.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? LogoUrl { get; set; }
        public List<User> Employers { get; set; }
        public List<Job> Jobs { get; set; }
    }
}