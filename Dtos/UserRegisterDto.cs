namespace JobBoard.Dtos
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; } // "Candidate" or "Employer"
    }
}