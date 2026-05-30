// This controller handles Welcome page and Assignments page
app.controller('AssignmentController', function($scope, $location, authService, assignmentService) {

    // Get current user info
    $scope.userRole = authService.getRole();       // "Teacher" or "Student"
    $scope.userName = authService.getUserName();
    $scope.userId = authService.getUserId();

    // Helper: check if current user is a Teacher
    $scope.isTeacher = function() {
        return $scope.userRole === 'Teacher';
    };

    // Helper: check if current user is a Student
    $scope.isStudent = function() {
        return $scope.userRole === 'Student';
    };

    $scope.assignments = [];
    $scope.message = '';
    $scope.isError = false;
    $scope.showForm = false;        // Toggle create/edit form
    $scope.showSubmitForm = false;  // Toggle submit form
    $scope.editMode = false;        // Are we editing or creating?
    $scope.formData = {};           // Holds form input
    $scope.submitData = {};         // Holds submission input
    $scope.selectedAssignmentId = null;
    $scope.submissions = [];
    $scope.showSubmissions = false;

    // Load all assignments when page loads
    $scope.loadAssignments = function() {
        assignmentService.getAll()
            .then(function(response) {
                $scope.assignments = response.data;
            })
            .catch(function(error) {
                $scope.message = 'Failed to load assignments.';
                $scope.isError = true;
            });
    };

    // Show the Create form
    $scope.showCreateForm = function() {
        $scope.showForm = true;
        $scope.editMode = false;
        $scope.formData = {};
        $scope.message = '';
    };

    // Show the Edit form with existing data
    $scope.showEditForm = function(assignment) {
        $scope.showForm = true;
        $scope.editMode = true;
        // Format date properly for input field
        $scope.formData = {
            assignmentId: assignment.assignmentId,
            title: assignment.title,
            description: assignment.description,
            dueDate: assignment.dueDate.substring(0, 10)
        };
        $scope.message = '';
    };

    // Cancel form
    $scope.cancelForm = function() {
        $scope.showForm = false;
        $scope.formData = {};
    };

    // CREATE or UPDATE assignment
    $scope.saveAssignment = function() {
        if ($scope.editMode) {
            // UPDATE
            assignmentService.update($scope.formData.assignmentId, $scope.formData)
                .then(function() {
                    $scope.message = 'Assignment updated successfully!';
                    $scope.isError = false;
                    $scope.showForm = false;
                    $scope.loadAssignments();
                })
                .catch(function() {
                    $scope.message = 'Failed to update assignment.';
                    $scope.isError = true;
                });
        } else {
            // CREATE
            assignmentService.create($scope.formData)
                .then(function() {
                    $scope.message = 'Assignment created successfully!';
                    $scope.isError = false;
                    $scope.showForm = false;
                    $scope.loadAssignments();
                })
                .catch(function() {
                    $scope.message = 'Failed to create assignment.';
                    $scope.isError = true;
                });
        }
    };

    // DELETE assignment
    $scope.deleteAssignment = function(id) {
        if (confirm('Are you sure you want to delete this assignment?')) {
            assignmentService.delete(id)
                .then(function() {
                    $scope.message = 'Assignment deleted successfully!';
                    $scope.isError = false;
                    $scope.loadAssignments();
                })
                .catch(function() {
                    $scope.message = 'Failed to delete assignment.';
                    $scope.isError = true;
                });
        }
    };

    // Show submit form for a student
    $scope.showSubmitAssignment = function(assignmentId) {
        $scope.showSubmitForm = true;
        $scope.selectedAssignmentId = assignmentId;
        $scope.submitData = { assignmentId: assignmentId };
        $scope.message = '';
    };

    // SUBMIT assignment (Student)
    $scope.submitAssignment = function() {
        assignmentService.submit($scope.submitData)
            .then(function() {
                $scope.message = 'Assignment submitted successfully!';
                $scope.isError = false;
                $scope.showSubmitForm = false;
                $scope.submitData = {};
            })
            .catch(function() {
                $scope.message = 'Failed to submit assignment.';
                $scope.isError = true;
            });
    };

    // VIEW submissions (Teacher)
    $scope.viewSubmissions = function(assignmentId) {
        assignmentService.getSubmissions(assignmentId)
            .then(function(response) {
                $scope.submissions = response.data;
                $scope.showSubmissions = true;
                $scope.selectedAssignmentId = assignmentId;
            })
            .catch(function() {
                $scope.message = 'Failed to load submissions.';
                $scope.isError = true;
            });
    };

    // Load assignments on page start
    $scope.loadAssignments();
});