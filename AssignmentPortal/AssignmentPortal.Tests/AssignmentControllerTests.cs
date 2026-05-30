using AssignmentPortal.API.Controllers;
using AssignmentPortal.API.Models;
using AssignmentPortal.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace AssignmentPortal.Tests
{
    public class AssignmentControllerTests
    {
        private readonly Mock<IAssignmentRepository> _mockRepo;
        private readonly AssignmentsController _controller;

        public AssignmentControllerTests()
        {
            _mockRepo = new Mock<IAssignmentRepository>();
            _controller = new AssignmentsController(_mockRepo.Object);

            var teacherClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Teacher"),
                new Claim(ClaimTypes.Name, "John Teacher")
            };

            var identity = new ClaimsIdentity(teacherClaims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }

        // =============================================
        // TEST 1 — GetAll returns OK with a list
        // =============================================
        [Fact]
        public void GetAll_ReturnsOk_WithListOfAssignments()
        {
            // ARRANGE
            var fakeAssignments = new List<Assignment>
            {
                new Assignment { AssignmentId = 1, Title = "Math HW", Description = "Chapter 1", DueDate = DateTime.Now.AddDays(7), CreatedBy = 1 },
                new Assignment { AssignmentId = 2, Title = "Science HW", Description = "Chapter 2", DueDate = DateTime.Now.AddDays(5), CreatedBy = 1 }
            };

            _mockRepo.Setup(r => r.GetAllAssignments()).Returns(fakeAssignments);

            // ACT
            var result = _controller.GetAll();

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedList = Assert.IsType<List<Assignment>>(okResult.Value);
            Assert.Equal(2, returnedList.Count);
        }

        // =============================================
        // TEST 2 — GetById returns correct assignment
        // =============================================
        [Fact]
        public void GetById_ValidId_ReturnsOk_WithAssignment()
        {
            // ARRANGE
            var fakeAssignment = new Assignment
            {
                AssignmentId = 1,
                Title = "Math HW",
                Description = "Chapter 1",
                DueDate = DateTime.Now.AddDays(7),
                CreatedBy = 1
            };

            _mockRepo.Setup(r => r.GetAssignmentById(1)).Returns(fakeAssignment);

            // ACT
            var result = _controller.GetById(1);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Assignment>(okResult.Value);
            Assert.Equal("Math HW", returned.Title);
        }

        // =============================================
        // TEST 3 — GetById with invalid ID returns 404
        // =============================================
        [Fact]
        public void GetById_InvalidId_ReturnsNotFound()
        {
            // ARRANGE
            _mockRepo.Setup(r => r.GetAssignmentById(999)).Returns((Assignment)null);

            // ACT
            var result = _controller.GetById(999);

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // =============================================
        // TEST 4 — Create with valid data returns OK
        // =============================================
        [Fact]
        public void Create_ValidAssignment_ReturnsOk()
        {
            // ARRANGE
            var newAssignment = new Assignment
            {
                Title = "New Assignment",
                Description = "Do this task",
                DueDate = DateTime.Now.AddDays(10),
                CreatedBy = 1
            };

            _mockRepo.Setup(r => r.CreateAssignment(It.IsAny<Assignment>()));

            // ACT
            var result = _controller.Create(newAssignment);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        // =============================================
        // TEST 5 — Create with null data returns BadRequest
        // =============================================
        [Fact]
        public void Create_NullAssignment_ReturnsBadRequest()
        {
            // ACT
            var result = _controller.Create(null);

            // ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // =============================================
        // TEST 6 — Delete existing assignment returns OK
        // =============================================
        [Fact]
        public void Delete_ValidId_ReturnsOk()
        {
            // ARRANGE
            var fakeAssignment = new Assignment
            {
                AssignmentId = 1,
                Title = "Math HW",
                Description = "Chapter 1",
                DueDate = DateTime.Now.AddDays(7),
                CreatedBy = 1
            };

            _mockRepo.Setup(r => r.GetAssignmentById(1)).Returns(fakeAssignment);
            _mockRepo.Setup(r => r.DeleteAssignment(1));

            // ACT
            var result = _controller.Delete(1);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        // =============================================
        // TEST 7 — Delete non existing assignment returns 404
        // =============================================
        [Fact]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            // ARRANGE
            _mockRepo.Setup(r => r.GetAssignmentById(999)).Returns((Assignment)null);

            // ACT
            var result = _controller.Delete(999);

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // =============================================
        // TEST 8 — Update existing assignment returns OK
        // =============================================
        [Fact]
        public void Update_ValidAssignment_ReturnsOk()
        {
            // ARRANGE
            var existingAssignment = new Assignment
            {
                AssignmentId = 1,
                Title = "Old Title",
                Description = "Old Description",
                DueDate = DateTime.Now.AddDays(5),
                CreatedBy = 1
            };

            var updatedAssignment = new Assignment
            {
                Title = "New Title",
                Description = "New Description",
                DueDate = DateTime.Now.AddDays(10)
            };

            _mockRepo.Setup(r => r.GetAssignmentById(1)).Returns(existingAssignment);
            _mockRepo.Setup(r => r.UpdateAssignment(It.IsAny<Assignment>()));

            // ACT
            var result = _controller.Update(1, updatedAssignment);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        // =============================================
        // TEST 9 — Submit null returns BadRequest
        // =============================================
        [Fact]
        public void Submit_NullSubmission_ReturnsBadRequest()
        {
            // ARRANGE — simulate Student
            var studentClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "2"),
                new Claim(ClaimTypes.Role, "Student"),
                new Claim(ClaimTypes.Name, "Jane Student")
            };

            var identity = new ClaimsIdentity(studentClaims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // ACT
            var result = _controller.Submit(null);

            // ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // =============================================
        // TEST 10 — GetAll returns empty list (edge case)
        // =============================================
        [Fact]
        public void GetAll_ReturnsOk_WithEmptyList()
        {
            // ARRANGE
            _mockRepo.Setup(r => r.GetAllAssignments()).Returns(new List<Assignment>());

            // ACT
            var result = _controller.GetAll();

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedList = Assert.IsType<List<Assignment>>(okResult.Value);
            Assert.Empty(returnedList);
        }

        // =============================================
        // TEST 11 — GetAll throws exception
        // =============================================
        [Fact]
        public void GetAll_WhenRepositoryThrowsException_ExceptionIsPropagated()
        {
            // ARRANGE
            _mockRepo.Setup(r => r.GetAllAssignments())
                     .Throws(new Exception("Database connection failed"));

            // ACT + ASSERT
            Assert.Throws<Exception>(() => _controller.GetAll());
        }

        // =============================================
        // TEST 12 — GetById throws exception
        // =============================================
        [Fact]
        public void GetById_WhenRepositoryThrowsException_ExceptionIsPropagated()
        {
            // ARRANGE
            _mockRepo.Setup(r => r.GetAssignmentById(It.IsAny<int>()))
                     .Throws(new Exception("Database error"));

            // ACT + ASSERT
            Assert.Throws<Exception>(() => _controller.GetById(1));
        }

    } // END OF CLASS
} // END OF NAMESPACE