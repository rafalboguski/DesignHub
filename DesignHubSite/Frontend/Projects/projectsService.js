'use strict';
app.service('projectsService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getProjects = function () {
        return $http.get(apiUrl + '/projects');
    };

    this.getProject = function (id) {
        return $http.get(apiUrl + '/projects/' + id);
    };

    this.getPermission = function (projectId, userId) {
        return $http.get(apiUrl + '/projects/' + id);
    };

    this.inviteWatcher = function (projectId, userId) {
        return $http({
            method: 'POST',
            url: apiUrl + '/projects/' + projectId + '/inviteWatcher/' + userId,
            data: {},
            headers: { 'Content-Type': 'application/json' }
        });
    };

    this.createProject = function (project) {
        return $http({
            method: 'POST',
            url: apiUrl + '/projects',
            data: {
                'Name': project.name,
                'Description': project.description

            },
            headers: { 'Content-Type': 'application/json' }
        });
    };

    this.deleteProject = function (id) {
        return $http({
            method: 'DELETE',
            url: apiUrl + '/projects/' + id,
            data: {
            },
            headers: { 'Content-Type': 'application/json' }
        });
    };

    this.getNotes = function (id) {
        return $http.get(apiUrl + '/projects/' + id + '/notes');
    };

    this.addNote = function (projectId, note) {
        console.log('projectsService function addNote(' + projectId+ ', '+note+')');
        return $http({
            method: 'POST',
            url: apiUrl + '/projects/' + projectId + '/notes',
            data: {
                'text':note,
                'lol':'lolol'
            },
            headers: { 'Content-Type': 'application/json' }
        });
    };

    this.removeNote = function (projectId, noteId) {
        return $http.delete(apiUrl + '/projects/' + projectId + '/notes/' + noteId)
    };


    this.getPermitedUsers = function (projectId) {
        return $http.get(apiUrl + '/users/permissions_in_project/' + projectId);
    };


    this.acceptProject = function (projectId) {
        return $http.post(apiUrl + '/projects/' + projectId + '/status/accept');
    };
    this.rejectProject = function (projectId) {
        return $http.post(apiUrl + '/projects/' + projectId + '/status/reject');
    };

}]);

//62937

//app.service('projectsService', function ($http) {

//    var apiUrl = 'http://localhost:4567';

//    this.Search = function() {
//        return $http.get(apiUrl + '/search/' + word);
//    };




//});