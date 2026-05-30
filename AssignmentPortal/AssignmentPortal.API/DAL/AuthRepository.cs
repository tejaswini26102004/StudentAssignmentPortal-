using AssignmentPortal.API.Models;
using AssignmentPortal.API.Repositories;
using Microsoft.Data.SqlClient;

namespace AssignmentPortal.API.DAL
{
    // This class handles all user authentication database operations
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Save a new user to the database
        public void RegisterUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO Users (Name, Email, PasswordHash, DateOfBirth, Designation) 
                                 VALUES (@Name, @Email, @PasswordHash, @DateOfBirth, @Designation)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Designation", user.Designation);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Find a user by their email address
        public User GetUserByEmail(string email)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Users WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserId = (int)reader["UserId"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Designation = reader["Designation"].ToString()
                            };
                        }
                    }
                }
            }
            return user;
        }
    }
}