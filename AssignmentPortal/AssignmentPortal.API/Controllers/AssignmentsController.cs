using AssignmentPortal.API.Models;
using AssignmentPortal.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AssignmentPortal.API.Controllers
{
    // This controller handles all Assignment CRUD operations
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints require a valid JWT token
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentsController(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        // GET: api/assignments — Both Teacher and Student can view
        [HttpGet]
        public IActionResult GetAll()
        {
            var assignments = _assignmentRepository.GetAllAssignments();
            return Ok(assignments);
        }

        // GET: api/assignments/5 — Both Teacher and Student can view
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var assignment = _assignmentRepository.GetAssignmentById(id);
            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }
            return Ok(assignment);
        }

        // POST: api/assignments — Only Teacher can create
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public IActionResult Create([FromBody] Assignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest("Assignment data is required.");
            }

            // Get the logged in teacher's UserId from the JWT token
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            assignment.CreatedBy = teacherId;

            _assignmentRepository.CreateAssignment(assignment);
            return Ok("Assignment created successfully.");
        }

        // PUT: api/assignments/5 — Only Teacher can update
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public IActionResult Update(int id, [FromBody] Assignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest("Assignment data is required.");
            }

            var existing = _assignmentRepository.GetAssignmentById(id);
            if (existing == null)
            {
                return NotFound("Assignment not found.");
            }

            assignment.AssignmentId = id;
            _assignmentRepository.UpdateAssignment(assignment);
            return Ok("Assignment updated successfully.");
        }

        // DELETE: api/assignments/5 — Only Teacher can delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public IActionResult Delete(int id)
        {
            var existing = _assignmentRepository.GetAssignmentById(id);
            if (existing == null)
            {
                return NotFound("Assignment not found.");
            }

            _assignmentRepository.DeleteAssignment(id);
            return Ok("Assignment deleted successfully.");
        }

        // POST: api/assignments/submit — Only Student can submit
        [HttpPost("submit")]
        [Authorize(Roles = "Student")]
        public IActionResult Submit([FromBody] Submission submission)
        {
            if (submission == null)
            {
                return BadRequest("Submission data is required.");
            }

            // Get the logged in student's UserId from the JWT token
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            submission.StudentId = studentId;

            _assignmentRepository.CreateSubmission(submission);
            return Ok("Assignment submitted successfully.");
        }

        // GET: api/assignments/5/submissions — Only Teacher can view submissions
        [HttpGet("{id}/submissions")]
        [Authorize(Roles = "Teacher")]
        public IActionResult GetSubmissions(int id)
        {
            var submissions = _assignmentRepository.GetSubmissionsByAssignment(id);
            return Ok(submissions);
        }
    }
}