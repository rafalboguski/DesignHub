﻿'use strict';
app.controller('projectsController', ['$scope', '$routeParams', '$location', 'Upload', '$timeout', 'projectsService', 'usersService',
    function ($scope, $routeParams, $location, Upload, $timeout, projectsService, usersService) {

        $scope.current_page = $scope.$parent.current_page;

        // project details
        $scope.projectId = $routeParams.projectId;
        $scope.project;

        // gallery
        $scope.projects = [];

        $scope.visibleProjectsNum = 8;

        //-- Init --------------------------------------

        $scope.init = function () {

            $scope.getProject();

            $scope.getUsers();
            $scope.getNotes();

            Materialize.toast('INIT', 2000);
            $('.datepicker').pickadate({
                selectMonths: true, // Creates a dropdown to control month
                selectYears: 15 // Creates a dropdown of 15 years to control year
            });





        }


        $scope.getProject = function () {
            projectsService.getProject($scope.projectId).then(function (results) {

                if (results.data == 'null') {
                    console.log('project 404');
                    $scope.$parent.href('/404');
                }

                $scope.project = results.data;

                $scope.$parent.project_name = results.data.name;
                $scope.$parent.project_id = $scope.projectId;
                $scope.$parent.project = results.data;


            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.getProjects = function () {
            projectsService.getProjects().then(function (results) {

                $scope.projects = results.data.reverse();


                if ($scope.projects.length == 0) {

                    $scope.galleryLoadded = true;
                }

                angular.forEach($scope.projects, function (node) {

                    node.headImage = (node.headImage && node.headImage != "null") ? node.headImage.substring(0, node.headImage.length) : null;

                });





            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.galleryLoadded = false;

        $scope.galleryLoaddingFinished = function () {

            Materialize.toast('Projects loadded', 500);
            $('.toast').addClass('green');
            $scope.galleryLoadded = true;

        }

        //--Project Details --------------------------------------

        // todo: get from project only
        $scope.users;
        $scope.getUsers = function () {

            projectsService.getPermitedUsers($scope.projectId).then(function (results) {
                $scope.users = results.data;
                console.log('getUsers: ', $scope.users);

                $('.tooltipped').tooltip({ delay: 10 });

            }, function (error) {
                alert(error.data.message);
            });

        }

        $scope.rejectProject = function (id) {

            projectsService.rejectProject(id).then(function (results) {
                $scope.getProject();
                Materialize.toast('Scope rejected', 2000);
            }, function (error) {
                console.log(error);
                alert(error.data);
            });
        }
        $scope.acceptProject = function (id) {

            projectsService.acceptProject(id).then(function (results) {
                $scope.getProject();
                Materialize.toast('Scope rejected', 2000);
            }, function (error) {
                console.log(error);
                alert(error.data);
            });
        }

        $scope.getNotes = function () {
            projectsService.getNotes($scope.projectId).then(function (results) {
                $scope.notes = results.data;
                angular.forEach($scope.notes, function (note) {

                    var lines = note.content.split('\n');
                    note.content = lines;

                });

            }, function (error) {
                console.log(error);
                alert(error.data);
            });
        }

        $scope.addNote = function (note) {
            projectsService.addNote($scope.projectId, note).then(function (results) {
                $scope.getNotes();
                $scope.note = "";
                Materialize.toast('Note added', 2000);
            }, function (error) {
                console.log(error);
                alert(error.data);
            });
        }

        $scope.removeNote = function (id) {
            projectsService.removeNote($scope.projectId, id).then(function (results) {
                $scope.getNotes();
                Materialize.toast('Note removed', 2000);
            }, function (error) {
                console.log(error);
                alert(error.data);
            });
        }

        $scope.requirements = [];
        $scope.createRequirement = function () {

            var $input = $('.datepicker').pickadate();

            // Use the picker object directly.
            var picker = $input.pickadate('picker');

            console.log($input);
            console.log(picker);
            console.log(picker.get('select'));
            var date = picker.get('select');
            var year = date.year;
            var month = date.month + 1;
            var day = date.date;

            requirementsService.findPersons(text).then(function (results) {

                if (text.length < 1) {
                    $scope.addPerson.selected = null;
                }
                else {
                    $scope.addPerson.searchResult = results.data;

                }
            });


        }


        // -- Add new Person 
        $scope.addPerson = {};

        $scope.addPersonSearch = function (text) {

            usersService.findPersons(text).then(function (results) {

                if (text.length < 1) {
                    $scope.addPerson.selected = null;
                }
                else {
                    $scope.addPerson.searchResult = results.data;

                }
            });

        }

        $scope.addPersonSelect = function (person) {

            $scope.addPerson.selected = person;

        }

        $scope.addPersonFinalize = function (data) {

            data.projectId = $scope.projectId;

            usersService.assignToProject(data).then(function (results) {
                Materialize.toast('Saved', 1500);
                data = null;
                $('#AddPersonModal').closeModal();
                $scope.addPerson.selected = null;
                $scope.getUsers();
            }, function (error) {
                Materialize.toast(error.data.message, 3000);
            });

        }






        //---------------------------------------------

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

        $scope.deleteProject = function (id) {

            projectsService.deleteProject(id).then(function (results) {
                if (results.status == 200) {
                    $location.path('/projects');
                    alert('Deleted');
                }
            }, function (error) {
                alert(error.data.message);
            });
        }

        // delay search
        var tempFilterText = '', filterTextTimeout;
        $scope.$watch('tabFilter.name', function (val) {
            if (filterTextTimeout)
                $timeout.cancel(filterTextTimeout);

            tempFilterText = val;
            filterTextTimeout = $timeout(function () {
                if (!$scope.tabFilter) {
                    $scope.tabFilter = {};
                }
                $scope.tabFilter.name = tempFilterText;
            }, 1000); // delay ms
        })


    }]);



