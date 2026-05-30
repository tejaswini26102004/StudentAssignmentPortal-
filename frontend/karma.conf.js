// This file tells Karma how to run our Jasmine tests
module.exports = function(config) {
    config.set({
        // Use Chrome browser to run tests
        browsers: ['Chrome'],

        // Use Jasmine as the testing framework
        frameworks: ['jasmine'],

        // All files Karma needs to load — ORDER MATTERS!
        files: [
            // First load AngularJS
            'node_modules/angular/angular.min.js',
            'node_modules/angular-route/angular-route.min.js',

            // Load angular-mocks for testing ($httpBackend etc.)
            'node_modules/angular-mocks/angular-mocks.js',

            // Load our app files
            'js/app.js',
            'js/authService.js',
            'js/assignmentService.js',
            'js/authController.js',
            'js/assignmentController.js',

            // Load our test files
            'tests/*.spec.js'
        ],

        // Show detailed results
        reporters: ['progress'],

        // Keep running and watch for changes
        singleRun: true
    });
};