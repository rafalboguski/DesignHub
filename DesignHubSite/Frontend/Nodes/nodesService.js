﻿'use strict';
app.service('nodesService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getNodes = function (projectId) {
        return $http.get(apiUrl + '/nodes/project/' + projectId);
    };

    this.getNode = function (id) {
        return $http.get(apiUrl + '/nodes/' + id);
    };

    this.saveNode = function (id, changes) {
                 
        // zmiana pozycji na grafie
        if (typeof (changes.position) != 'undefined') {
            return $http({
                method: 'PUT',
                url: apiUrl + '/nodes/' + id,
                data: {
                    'positionX': changes.position.x,
                    'positionY': changes.position.y

                },
                headers: { 'Content-Type': 'application/json' }
            });
        }
    };

   


}]);
