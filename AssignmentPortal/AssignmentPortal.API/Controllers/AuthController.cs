using AssignmentPortal.API.Models;
using AssignmentPortal.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssignmentPortal.API.Controllers
{
    // This controller handles Register and Login
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // These are injected - we don't create them manually
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Check if email already exists
            var existingUser = _authRepository.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already registered.");
            }

            // Hash the password before saving - never store plain text passwords!
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create a new User object
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = hashedPassword,
                DateOfBirth = request.DateOfBirth,
                Designation = request.Designation
            };

            // Save to database
            _authRepository.RegisterUser(user);

            return Ok("Registration successful.");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Find user by email
            var user = _authRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Check if password matches the hashed password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { token = token, role = user.Designation, name = user.Name, userId = user.UserId });
        }

        // This method creates the JWT token
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            // Claims are pieces of information stored inside the token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Designation), // "Teacher" or "Student"
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}