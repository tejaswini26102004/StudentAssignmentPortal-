// Tests for AssignmentController
describe('AssignmentController', function() {

    var $controller;
    var $scope;
    var $location;
    var mockAuthService;
    var mockAssignmentService;

    // Sample fake assignments
    var fakeAssignments = [
        { assignmentId: 1, title: 'Math HW', description: 'Chapter 1', dueDate: '2026-06-01T00:00:00' },
        { assignmentId: 2, title: 'Science HW', description: 'Chapter 2', dueDate: '2026-06-05T00:00:00' }
    ];

    beforeEach(function() {
        module('assignmentApp');

        inject(function(_$controller_, _$rootScope_, _$location_) {
            $controller = _$controller_;
            $scope = _$rootScope_.$new();
            $location = _$location_;
        });

        // Fake authService — simulating a Teacher by default
        mockAuthService = {
            getRole: function() { return 'Teacher'; },
            getUserName: function() { return 'John Teacher'; },
            getUserId: function() { return '1'; },
            isLoggedIn: function() { return true; }
        };

        // Fake assignmentService
        mockAssignmentService = {
            getAll: function() {
                return {
                    then: function(successFn) {
                        successFn({ data: fakeAssignments });
                        return { catch: function() {} };
                    }
                };
            },
            create: function() {
                return {
                    then: function(successFn) {
                        successFn({ data: 'Assignment created successfully.' });
                        return { catch: function() {} };
                    }
                };
            },
            update: function() {
                return {
                    then: function(successFn) {
                        successFn({ data: 'Assignment updated successfully.' });
                        return { catch: function() {} };
                    }
                };
            },
            delete: function() {
                return {
                    then: function(successFn) {
                        successFn({ data: 'Assignment deleted successfully.' });
                        return { catch: function() {} };
                    }
                };
            },
            submit: function() {
                return {
                    then: function(successFn) {
                        successFn({ data: 'Submitted successfully.' });
                        return { catch: function() {} };
                    }
                };
            },
            getSubmissions: function() {
                return {
                    then: function(successFn) {
                        successFn({ data: [] });
                        return { catch: function() {} };
                    }
                };
            }
        };
    });

    // Helper function to create the controller
    function createController() {
        return $controller('AssignmentController', {
            $scope: $scope,
            $location: $location,
            authService: mockAuthService,
            assignmentService: mockAssignmentService
        });
    }

    // =============================================
    // TEST 1 — Assignments load on startup
    // =============================================
    it('should load assignments on startup', function() {
        createController();

        // Check assignments were loaded
        expect($scope.assignments.length).toBe(2);
        expect($scope.assignments[0].title).toBe('Math HW');
    });

    // =============================================
    // TEST 2 — Teacher role is detected correctly
    // =============================================
    it('should detect Teacher role correctly', function() {
        createController();

        expect($scope.isTeacher()).toBe(true);
        expect($scope.isStudent()).toBe(false);
    });

    // =============================================
    // TEST 3 — Student role is detected correctly
    // =============================================
    it('should detect Student role correctly', function() {
        // Override to simulate Student
        mockAuthService.getRole = function() { return 'Student'; };

        createController();

        expect($scope.isStudent()).toBe(true);
        expect($scope.isTeacher()).toBe(false);
    });

    // =============================================
    // TEST 4 — Show create form sets editMode false
    // =============================================
    it('should show create form with editMode false', function() {
        createController();

        $scope.showCreateForm();

        expect($scope.showForm).toBe(true);
        expect($scope.editMode).toBe(false);
        expect($scope.formData).toEqual({});
    });

    // =============================================
    // TEST 5 — Show edit form sets editMode true
    // =============================================
    it('should show edit form with editMode true and prefill data', function() {
        createController();

        var assignment = {
            assignmentId: 1,
            title: 'Math HW',
            description: 'Chapter 1',
            dueDate: '2026-06-01T00:00:00'
        };

        $scope.showEditForm(assignment);

        expect($scope.showForm).toBe(true);
        expect($scope.editMode).toBe(true);
        expect($scope.formData.title).toBe('Math HW');
    });

    // =============================================
    // TEST 6 — Cancel form hides form
    // =============================================
    it('should hide form when cancel is clicked', function() {
        createController();

        $scope.showForm = true;
        $scope.cancelForm();

        expect($scope.showForm).toBe(false);
    });

    // =============================================
    // TEST 7 — Create assignment shows success message
    // =============================================
    it('should show success message after creating assignment', function() {
        createController();

        $scope.editMode = false;
        $scope.formData = {
            title: 'New Assignment',
            description: 'Do this',
            dueDate: '2026-06-10'
        };

        $scope.saveAssignment();

        expect($scope.message).toBe('Assignment created successfully!');
        expect($scope.isError).toBe(false);
    });

    // =============================================
    // TEST 8 — Submit form shows for correct assignment
    // =============================================
    it('should show submit form for the selected assignment', function() {
        createController();

        $scope.showSubmitAssignment(1);

        expect($scope.showSubmitForm).toBe(true);
        expect($scope.selectedAssignmentId).toBe(1);
        expect($scope.submitData.assignmentId).toBe(1);
    });

    // =============================================
    // TEST 9 — Student submits assignment successfully
    // =============================================
    it('should show success message after student submits assignment', function() {
        createController();

        $scope.submitData = {
            assignmentId: 1,
            submissionText: 'My answer here'
        };

        $scope.submitAssignment();

        expect($scope.message).toBe('Assignment submitted successfully!');
        expect($scope.showSubmitForm).toBe(false);
    });

    // =============================================
    // TEST 10 — Failed API call shows error message
    // =============================================
    it('should show error message when API fails to load assignments', function() {

        // Override getAll to simulate failure
        mockAssignmentService.getAll = function() {
            return {
                then: function(successFn) {
                    return {
                        catch: function(errorFn) {
                            errorFn({ status: 500 });
                        }
                    };
                }
            };
        };

        createController();

        expect($scope.message).toBe('Failed to load assignments.');
        expect($scope.isError).toBe(true);
    });
});