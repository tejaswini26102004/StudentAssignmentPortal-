using AssignmentPortal.API.Models;

namespace AssignmentPortal.API.Repositories
{
    // This is the "contract" - it defines what database operations are available
    // We use an interface so we can easily mock it in unit tests
    public interface IAssignmentRepository
    {
        // Assignment operations
        List<Assignment> GetAllAssignments();
        Assignment GetAssignmentById(int id);
        void CreateAssignment(Assignment assignment);
        void UpdateAssignment(Assignment assignment);
        void DeleteAssignment(int id);

        // Submission operations
        void CreateSubmission(Submission submission);
        List<Submission> GetSubmissionsByAssignment(int assignmentId);
    }
}