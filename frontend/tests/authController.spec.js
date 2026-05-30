// Tests for AuthController
// 'describe' groups related tests together
describe('AuthController', function() {

    // These variables will hold our fake versions of things
    var $controller;
    var $scope;
    var $location;
    var mockAuthService;

    // beforeEach runs before every single test
    beforeEach(function() {

        // Load our AngularJS app
        module('assignmentApp');

        // Inject AngularJS testing tools
        inject(function(_$controller_, _$rootScope_, _$location_) {
            $controller = _$controller_;
            $scope = _$rootScope_.$new();
            $location = _$location_;
        });

        // Create a fake authService so we don't make real API calls
        mockAuthService = {
            login: function() {},
            register: function() {},
            saveToken: function() {},
            isLoggedIn: function() { return false; },
            getUserName: function() { return ''; },
            logout: function() {}
        };
    });

    // =============================================
    // TEST 1 — Controller loads with empty form data
    // =============================================
    it('should initialize with empty loginData and registerData', function() {

        // Create the controller with our fake service
        $controller('AuthController', {
            $scope: $scope,
            $location: $location,
            authService: mockAuthService
        });

        // Check that form data starts empty
        expect($scope.loginData).toBeDefined();
        expect($scope.registerData).toBeDefined();
        expect($scope.message).toBe('');
    });

    // =============================================
    // TEST 2 — Successful login redirects to welcome
    // =============================================
    it('should redirect to /welcome on successful login', function() {

        // Make the fake login return a successful response
        mockAuthService.login = function() {
            return {
                then: function(successFn) {
                    // Simulate success response from API
                    successFn({
                        data: {
                            token: 'fake-token-123',
                            role: 'Teacher',
                            name: 'John Teacher',
                            userId: 1
                        }
                    });
                    // Return object with catch so it doesn't break
                    return { catch: function() {} };
                }
            };
        };

        $controller('AuthController', {
            $scope: $scope,
            $location: $location,
            authService: mockAuthService
        });

        // Call login
        $scope.loginData = { email: 'teacher@test.com', password: 'Test@123' };
        $scope.login();

        // Check that it redirected to /welcome
        expect($location.path()).toBe('/welcome');
    });

    // =============================================
    // TEST 3 — Failed login shows error message
    // =============================================
    it('should show error message on failed login', function() {

        // Make the fake login return a failure
        mockAuthService.login = function() {
            return {
                then: function(successFn) {
                    // Return object with catch that calls error
                    return {
                        catch: function(errorFn) {
                            errorFn({ status: 401 });
                        }
                    };
                }
            };
        };

        $controller('AuthController', {
            $scope: $scope,
            $location: $location,
            authService: mockAuthService
        });

        $scope.loginData = { email: 'wrong@test.com', password: 'wrongpass' };
        $scope.login();

        // Check error message is shown
        expect($scope.message).toBe('Invalid email or password. Please try again.');
        expect($scope.isError).toBe(true);
    });

    // =============================================
    // TEST 4 — Successful registration shows success message
    // =============================================
    it('should show success message on successful registration', function() {

        mockAuthService.register = function() {
            return {
                then: function(successFn) {
                    successFn({ data: 'Registration successful.' });
                    return { catch: function() {} };
                }
            };
        };

        $controller('AuthController', {
            $scope: $scope,
            $location: $location,
            authService: mockAuthService
        });

        $scope.registerData = {
            name: 'Jane Student',
            email: 'student@test.com',
            password: 'Test@123',
            designation: 'Student'
        };
        $scope.register();

        // Check success message
        expect($scope.message).toBe('Registration successful! Please login.');
        expect($scope.isError).toBe(false);
    });

    // =============================================
    // TEST 5 — Failed registration shows error message
    // =============================================
    it('should show error message on failed registration', function() {

        mockAuthService.register = function() {
            return {
                then: function(successFn) {
                    return {
                        catch: function(errorFn) {
                            errorFn({ data: 'Email already registered.' });
                        }
                    };
                }
            };
        };

        $controller('AuthController', {
            $scope: $scope,
            $location: $location,
            authService: mockAuthService
        });

        $scope.register();

        expect($scope.message).toBe('Email already registered.');
        expect($scope.isError).toBe(true);
    });
});