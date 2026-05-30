namespace AssignmentPortal.API.Models
{
    // This is used when a user tries to log in
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}