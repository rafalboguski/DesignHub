'use strict';
app.controller('projectsController', ['$scope', '$routeParams', 'Upload', '$timeout', 'projectsService',
    function ($scope, $routeParams, Upload, $timeout, projectsService) {

        $scope.current_page = $scope.$parent.current_page;

        $scope.projectId = $routeParams.projectId;
        $scope.project;
        $scope.projects = [];
        $scope.desc = true;


        $scope.getProject = function () {
            projectsService.getProject($scope.projectId).then(function (results) {
                $scope.$parent.current_page = 'dupa';
                $scope.project = results.data;

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.getProjects = function () {
            projectsService.getProjects().then(function (results) {

                $scope.projects = results.data;

            }, function (error) {
                alert(error.data.message);
            });
        }

       

        $scope.inviteWatcher = function (projectId, userId) {
            projectsService.inviteWatcher(projectId, userId).then(function (results) {

                if (results.status == 200) {
                    alert('ok');
                }

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.uploadFiles = function (projectId, file) {
            $scope.f = file;
            if (file && !file.$error) {
                file.upload = Upload.upload({
                    url: projectsService.uploadImageAdress(projectId),
                    file: file
                });

                file.upload.then(function (response) {
                    $timeout(function () {
                        file.result = response.data;
                        $scope.getProjects();
                    });
                }, function (response) {
                    if (response.status > 0)
                        $scope.errorMsg = response.status + ': ' + response.data;
                });

                file.upload.progress(function (evt) {
                    file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                });
            }
        }


        $scope.createProject = function (user) {
            projectsService.createProject(user).then(function (results) {

                $scope.getProjects();

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.deleteProject = function (idx, id) {

            console.log(idx);
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