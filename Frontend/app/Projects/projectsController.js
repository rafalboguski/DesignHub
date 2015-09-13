'use strict';
app.controller('projectsController', ['$scope', 'projectsService', function ($scope, projectsService) {

    $scope.projects = [];

    projectsService.getProjects().then(function (results) {

        $scope.projects = results.data;

    }, function (error) {
        alert(error.data.message);
    });

}]);