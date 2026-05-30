// This service handles login, register, logout and storing the JWT token
app.service('authService', function($http) {

    // The base URL of our backend API
    var apiUrl = 'http://localhost:5235/api';

    // REGISTER a new user
    this.register = function(userData) {
        return $http.post(apiUrl + '/auth/register', userData);
    };

    // LOGIN and get JWT token back
    this.login = function(credentials) {
        return $http.post(apiUrl + '/auth/login', credentials);
    };

    // Save token and user info to localStorage after login
    this.saveToken = function(token, role, name, userId) {
        localStorage.setItem('token', token);
        localStorage.setItem('role', role);
        localStorage.setItem('name', name);
        localStorage.setItem('userId', userId);
    };

    // Get the saved token
    this.getToken = function() {
        return localStorage.getItem('token');
    };

    // Get the user's role (Teacher or Student)
    this.getRole = function() {
        return localStorage.getItem('role');
    };

    // Get the user's name
    this.getUserName = function() {
        return localStorage.getItem('name');
    };

    // Get the user's ID
    this.getUserId = function() {
        return localStorage.getItem('userId');
    };

    // Check if user is logged in
    this.isLoggedIn = function() {
        return localStorage.getItem('token') !== null;
    };

    // Logout — clear everything
    this.logout = function() {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        localStorage.removeItem('name');
        localStorage.removeItem('userId');
    };
});