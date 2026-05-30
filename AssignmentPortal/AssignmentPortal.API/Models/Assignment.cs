namespace AssignmentPortal.API.Models
{
    // This class represents an Assignment created by a Teacher
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int CreatedBy { get; set; } // Teacher's UserId
        public DateTime CreatedAt { get; set; }
    }
}
