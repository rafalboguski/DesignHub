'use strict';
app.service('usersService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getUsers = function () {
        return $http.get(apiUrl + '/users');
    };

    this.getUser = function (id) {
        return $http.get(apiUrl + '/users/' + id);
    };




    this.findPersons = function (text) {
        Materialize.toast(text, 500);
        return $http.get(apiUrl + '/users/find/' + text);
    };


    this.assignToProject = function (data) {
        return $http({
            method: 'POST',
            url: apiUrl + '/users/assignToProject/',
            data: {
                UserId: data.number,
                ProjectId: data.projectId,
                ProjectRole: data.role,
                Readonly: data.permissionA,
                Message: data.permissionB,
                LikeOrDislikeChanges: data.permissionC,
                AddMarkers: data.permissionD,
                AcceptNodes: data.permissionE,
                AcceptWholeProject: data.permissionF
            },
            headers: { 'Content-Type': 'application/json' }
        });
    };




}]);

