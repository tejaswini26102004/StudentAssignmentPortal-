namespace AssignmentPortal.API.Models
{
    // This class represents a Submission made by a Student
    public class Submission
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string SubmissionText { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}