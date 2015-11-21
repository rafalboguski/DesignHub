﻿var app = angular.module('AngularAuthApp', ['ngRoute','ngFileUpload']);




app.config(function ($routeProvider) {

 
    $routeProvider.when("/projects", {
        controller: "projectsController",
        templateUrl: "/FrontEnd/Projects/projects.html"
    });

    $routeProvider.when("/project/:projectId", {
        controller: "projectsController",
        templateUrl: "/FrontEnd/Projects/project.html"
    });
    $routeProvider.when("/project/:projectId/graph", {
        controller: "nodesCtrl",
        templateUrl: "/FrontEnd/Nodes/nodes.html"
        });

    $routeProvider.when("/settings", {
        
        templateUrl: "/FrontEnd/Projects/project.html"
    });

    $routeProvider.otherwise({ redirectTo: "/projects" });
});




