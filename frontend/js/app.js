// This is the main app file — it sets up routing (which URL shows which page)
var app = angular.module('assignmentApp', ['ngRoute']);

app.config(function($routeProvider) {
    $routeProvider

    // Login page
    .when('/login', {
        templateUrl: 'views/login.html',
        controller: 'AuthController'
    })

    // Register page
    .when('/register', {
        templateUrl: 'views/register.html',
        controller: 'AuthController'
    })

    // Welcome page (shown right after login)
    .when('/welcome', {
        templateUrl: 'views/welcome.html',
        controller: 'AssignmentController'
    })

    // Assignments list page
    .when('/assignments', {
        templateUrl: 'views/assignments.html',
        controller: 'AssignmentController'
    })

    // Default — go to login
    .otherwise({
        redirectTo: '/login'
    });
});
