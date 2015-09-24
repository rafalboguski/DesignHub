'use strict';
app.controller('projectsController', ['$scope', 'Upload', '$timeout', 'projectsService', function ($scope, Upload, $timeout, projectsService) {


    $scope.projects = [];

    $scope.getProjects = function () {
        projectsService.getProjects().then(function (results) {

            $scope.projects = results.data;

        }, function (error) {
            alert(error.data.message);
        });
    }

    //
    $scope.uploadFiles = function (projectId, file) {
        $scope.f = file;
        if (file && !file.$error) {
            file.upload = Upload.upload({
                url: 'http://localhost:54520//api/projects/' + projectId + '/image',
                file: file
            });

            file.upload.then(function (response) {
                $timeout(function () {
                    file.result = response.data;
                });
            }, function (response) {
                if (response.status > 0)
                    $scope.errorMsg = response.status + ': ' + response.data;
            });

            file.upload.progress(function (evt) {
                file.progress = Math.min(100, parseInt(100.0 *
                                                       evt.loaded / evt.total));
            });
        }
    }
    //

    $scope.createProject = function (user) {
        projectsService.createProject(user).then(function (results) {

            $scope.getProjects();

        }, function (error) {
            alert(error.data.message);
        });
    }

    $scope.deleteProject = function (idx, id) {

        projectsService.deleteProject(id).then(function (results) {
            $scope.getProjects();
            if (results.status == 200) {
                $scope.projects.splice(idx, 1);
                //alert('Deleted');
            }
        }, function (error) {
            alert(error.data.message);
        });
    }

}]);