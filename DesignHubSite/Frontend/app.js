﻿var app = angular.module('AngularAuthApp', ['ngRoute','ngFileUpload']);




app.config(function ($routeProvider) {

 
    $routeProvider.when("/projects", {
        controller: "projectsController",
        templateUrl: "/FrontEnd/Projects/projects.html"
    });

    $routeProvider.when("/myprojects", {
        controller: "myProjectsController",
        templateUrl: "/FrontEnd/MyProjects/myprojects.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});




