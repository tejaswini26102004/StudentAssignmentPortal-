namespace AssignmentPortal.API.Models
{
    // This class represents a User (Student or Teacher)
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Designation { get; set; } // "Teacher" or "Student"
    }
}