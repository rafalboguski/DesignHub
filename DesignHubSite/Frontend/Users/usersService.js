'use strict';
app.service('usersService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getUsers = function () {
        return $http.get(apiUrl + '/users');
    };

    this.getUser = function (id) {
        return $http.get(apiUrl + '/users/' + id);
    };




    this.findUses = function (text) {
        return $http.get(apiUrl + '/users/find' + text);
    };


    this.inviteWatcher = function (projectId, userId) {
        return $http({
            method: 'POST',
            url: apiUrl + '/projects/' + projectId + '/inviteWatcher/' + userId,
            data: {},
            headers: { 'Content-Type': 'application/json' }
        });
    };




}]);

