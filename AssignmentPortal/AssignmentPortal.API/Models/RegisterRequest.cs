namespace AssignmentPortal.API.Models
{
    // This is used when a new user registers
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Designation { get; set; } // "Teacher" or "Student"
    }
}