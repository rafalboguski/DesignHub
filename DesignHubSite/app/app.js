var app = angular.module('AngularAuthApp', ['ngRoute']);

app.config(function ($routeProvider) {

 
    $routeProvider.when("/projects", {
        controller: "projectsController",
        templateUrl: "/app/Projects/projects.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});




