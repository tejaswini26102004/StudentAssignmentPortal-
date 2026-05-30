using AssignmentPortal.API.Models;
using AssignmentPortal.API.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AssignmentPortal.API.DAL
{
    // This class does the actual database work using ADO.NET
    // It talks directly to SQL Server using raw SQL queries
    public class AssignmentRepository : IAssignmentRepository
    {
        // Connection string is injected from appsettings.json
        private readonly string _connectionString;

        public AssignmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // GET ALL ASSIGNMENTS
        public List<Assignment> GetAllAssignments()
        {
            // Create an empty list to store results
            var assignments = new List<Assignment>();

            // Open connection to SQL Server
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Write the SQL query
                string query = "SELECT * FROM Assignments";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Read the results row by row
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Map each row to an Assignment object
                            assignments.Add(new Assignment
                            {
                                AssignmentId = (int)reader["AssignmentId"],
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                DueDate = (DateTime)reader["DueDate"],
                                CreatedBy = (int)reader["CreatedBy"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            });
                        }
                    }
                }
            }
            return assignments;
        }

        // GET ALL ASSIGNMENTS USING SqlDataAdapter + DataSet
        // This is an alternative way to read data - disconnected architecture
        // SqlDataAdapter fills a DataSet and closes the connection automatically
        public List<Assignment> GetAllAssignmentsUsingDataAdapter()
        {
            var assignments = new List<Assignment>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Assignments";

                // SqlDataAdapter automatically opens and closes connection
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                // DataSet is like a temporary in-memory database
                DataSet dataSet = new DataSet();

                // Fill the DataSet with results from the query
                adapter.Fill(dataSet, "Assignments");

                // Loop through each row in the DataSet
                foreach (DataRow row in dataSet.Tables["Assignments"].Rows)
                {
                    assignments.Add(new Assignment
                    {
                        AssignmentId = (int)row["AssignmentId"],
                        Title = row["Title"].ToString(),
                        Description = row["Description"].ToString(),
                        DueDate = (DateTime)row["DueDate"],
                        CreatedBy = (int)row["CreatedBy"],
                        CreatedAt = (DateTime)row["CreatedAt"]
                    });
                }
            }
            return assignments;
        }

        // GET SINGLE ASSIGNMENT BY ID
        public Assignment GetAssignmentById(int id)
        {
            Assignment assignment = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Use parameterized query to prevent SQL injection
                string query = "SELECT * FROM Assignments WHERE AssignmentId = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // @Id is a parameter - this is safe and prevents SQL injection
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            assignment = new Assignment
                            {
                                AssignmentId = (int)reader["AssignmentId"],
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                DueDate = (DateTime)reader["DueDate"],
                                CreatedBy = (int)reader["CreatedBy"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };
                        }
                    }
                }
            }
            return assignment;
        }

        // CREATE ASSIGNMENT
        public void CreateAssignment(Assignment assignment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO Assignments (Title, Description, DueDate, CreatedBy) 
                                 VALUES (@Title, @Description, @DueDate, @CreatedBy)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", assignment.Title);
                    cmd.Parameters.AddWithValue("@Description", assignment.Description);
                    cmd.Parameters.AddWithValue("@DueDate", assignment.DueDate);
                    cmd.Parameters.AddWithValue("@CreatedBy", assignment.CreatedBy);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // UPDATE ASSIGNMENT
        public void UpdateAssignment(Assignment assignment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"UPDATE Assignments 
                                 SET Title = @Title, Description = @Description, DueDate = @DueDate 
                                 WHERE AssignmentId = @AssignmentId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", assignment.Title);
                    cmd.Parameters.AddWithValue("@Description", assignment.Description);
                    cmd.Parameters.AddWithValue("@DueDate", assignment.DueDate);
                    cmd.Parameters.AddWithValue("@AssignmentId", assignment.AssignmentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE ASSIGNMENT
        public void DeleteAssignment(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // First delete related submissions (because of foreign key)
                string deleteSubmissions = "DELETE FROM Submissions WHERE AssignmentId = @Id";
                using (SqlCommand cmd = new SqlCommand(deleteSubmissions, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                // Then delete the assignment
                string deleteAssignment = "DELETE FROM Assignments WHERE AssignmentId = @Id";
                using (SqlCommand cmd = new SqlCommand(deleteAssignment, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // CREATE SUBMISSION
        public void CreateSubmission(Submission submission)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO Submissions (AssignmentId, StudentId, SubmissionText) 
                                 VALUES (@AssignmentId, @StudentId, @SubmissionText)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssignmentId", submission.AssignmentId);
                    cmd.Parameters.AddWithValue("@StudentId", submission.StudentId);
                    cmd.Parameters.AddWithValue("@SubmissionText", submission.SubmissionText);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // GET SUBMISSIONS BY ASSIGNMENT
        public List<Submission> GetSubmissionsByAssignment(int assignmentId)
        {
            var submissions = new List<Submission>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Submissions WHERE AssignmentId = @AssignmentId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submissions.Add(new Submission
                            {
                                SubmissionId = (int)reader["SubmissionId"],
                                AssignmentId = (int)reader["AssignmentId"],
                                StudentId = (int)reader["StudentId"],
                                SubmissionText = reader["SubmissionText"].ToString(),
                                SubmittedAt = (DateTime)reader["SubmittedAt"]
                            });
                        }
                    }
                }
            }
            return submissions;
        }
    }
}