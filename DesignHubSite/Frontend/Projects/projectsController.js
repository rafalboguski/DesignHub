'use strict';
app.controller('projectsController', ['$scope', '$routeParams','$location', 'Upload', '$timeout', 'projectsService',
    function ($scope,$routeParams, $location, Upload, $timeout, projectsService) {

        $scope.current_page = $scope.$parent.current_page;

        // project details
        $scope.projectId = $routeParams.projectId;
        $scope.project;

        // gallery
        $scope.projects = [];
    
        $scope.visibleProjectsNum = 8;


        $scope.getProject = function () {
            projectsService.getProject($scope.projectId).then(function (results) {
              
                $scope.project = results.data;
               
                $scope.$parent.project_name = results.data.name;
                $scope.$parent.project_id = $scope.projectId;


            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.getProjects = function () {
            projectsService.getProjects().then(function (results) {

                $scope.projects = results.data.reverse();


                angular.forEach($scope.projects, function (node) {

                    node.headImage = (node.headImage != "null") ? node.headImage.substring(0, node.headImage.length ) : null;

                });





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


        $scope.createProject = function (project) {
           
            projectsService.createProject(project).then(function (results) {
                $scope.getProjects();
                project.name = '';
                project.description = '';
                $('#AddProjectModal').closeModal();

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.deleteProject = function ( id) {
          
            projectsService.deleteProject(id).then(function (results) {
                if (results.status == 200) {
                    $location.path('/projects');
                    alert('Deleted');
                }
            }, function (error) {
                alert(error.data.message);
            });
        }


    }]);



