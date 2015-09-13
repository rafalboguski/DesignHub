'use strict';
app.factory('projectsService', ['$http', function ($http) {

    var serviceBase = 'http://localhost:62937/';
    var projectsServiceFactory = {};

    var _getProjects = function () {

        return $http.get(serviceBase + 'api/projects').then(function (results) {
            return results;
        });
    };

    projectsServiceFactory.getProjects = _getProjects;

    return projectsServiceFactory;

}]);