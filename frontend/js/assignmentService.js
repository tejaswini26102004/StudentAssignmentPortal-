// This service handles all assignment API calls
// It automatically adds the JWT token to every request
app.service('assignmentService', function($http, authService) {

    var apiUrl = 'http://localhost:5235/api';
    // Helper function to build headers with JWT token
    function getHeaders() {
        return {
            headers: {
                'Authorization': 'Bearer ' + authService.getToken()
            }
        };
    }

    // GET all assignments
    this.getAll = function() {
        return $http.get(apiUrl + '/assignments', getHeaders());
    };

    // GET single assignment by id
    this.getById = function(id) {
        return $http.get(apiUrl + '/assignments/' + id, getHeaders());
    };

    // CREATE new assignment (Teacher only)
    this.create = function(assignment) {
        return $http.post(apiUrl + '/assignments', assignment, getHeaders());
    };

    // UPDATE assignment (Teacher only)
    this.update = function(id, assignment) {
        return $http.put(apiUrl + '/assignments/' + id, assignment, getHeaders());
    };

    // DELETE assignment (Teacher only)
    this.delete = function(id) {
        return $http.delete(apiUrl + '/assignments/' + id, getHeaders());
    };

    // SUBMIT assignment (Student only)
    this.submit = function(submission) {
        return $http.post(apiUrl + '/assignments/submit', submission, getHeaders());
    };

    // GET submissions for an assignment (Teacher only)
    this.getSubmissions = function(assignmentId) {
        return $http.get(apiUrl + '/assignments/' + assignmentId + '/submissions', getHeaders());
    };
});