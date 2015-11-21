'use strict';
app.service('nodesService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getNodes = function (projectId) {
        return $http.get(apiUrl + '/nodes/project/' + projectId);
    };

    this.getNode = function (id) {
        return $http.get(apiUrl + '/nodes/' + id);
    };



}]);

