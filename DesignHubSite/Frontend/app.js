var app = angular.module('AngularAuthApp', ['ngRoute','ngFileUpload']);




app.config(function ($routeProvider) {

 
    $routeProvider.when("/projects", {
        controller: "projectsController",
        templateUrl: "/FrontEnd/Projects/projects.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});




