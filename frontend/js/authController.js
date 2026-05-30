// This controller handles Login and Register pages
app.controller('AuthController', function($scope, $location, authService) {

    // These objects hold the form data
    $scope.loginData = {};
    $scope.registerData = {};
    $scope.message = '';
    $scope.isError = false;

    // Called when user clicks Login button
    $scope.login = function() {
        authService.login($scope.loginData)
            .then(function(response) {
                // Save the token and user info
                authService.saveToken(
                    response.data.token,
                    response.data.role,
                    response.data.name,
                    response.data.userId
                );
                // Go to welcome page
                $location.path('/welcome');
            })
            .catch(function(error) {
                $scope.message = 'Invalid email or password. Please try again.';
                $scope.isError = true;
            });
    };

    // Called when user clicks Register button
    $scope.register = function() {
        authService.register($scope.registerData)
            .then(function(response) {
                $scope.message = 'Registration successful! Please login.';
                $scope.isError = false;
                // Go to login after 1.5 seconds
                setTimeout(function() {
                    $location.path('/login');
                    $scope.$apply();
                }, 1500);
            })
            .catch(function(error) {
                $scope.message = error.data || 'Registration failed. Please try again.';
                $scope.isError = true;
            });
    };
});