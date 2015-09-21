'use strict';
app.controller('projectsController', ['$scope', 'projectsService', function ($scope, projectsService) {


    $scope.projects = [];

    $scope.getProjects = function () {
        projectsService.getProjects().then(function (results) {

            $scope.projects = results.data;

        }, function (error) {
            alert(error.data.message);
        });
    }



    $scope.createProject = function (user) {
        projectsService.createProject(user).then(function (results) {

            $scope.getProjects();

        }, function (error) {
            alert(error.data.message);
        });

    }

}]);