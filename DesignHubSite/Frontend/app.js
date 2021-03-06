﻿var app = angular.module('AngularAuthApp', ['ngRoute', 'ngFileUpload']);




app.config(function ($routeProvider) {


    $routeProvider.when("/projects", {
        controller: "projectsController",
        templateUrl: "/FrontEnd/Projects/projects.html"
    });

    $routeProvider.when("/project/:projectId/notifications", {
        controller: "notificationsCtrl",
        templateUrl: "/FrontEnd/Notifications/notifications.html"
    });
    $routeProvider.when("/project/:projectId", {
        controller: "projectsController",
        templateUrl: "/FrontEnd/Projects/project.html"
    });
    $routeProvider.when("/project/:projectId/graph/:selectedNodeId", {
        controller: "nodesCtrl",
        templateUrl: "/FrontEnd/Nodes/nodes.html"
    });
    $routeProvider.when("/project/:projectId/markers/:nodeId", {
        controller: "markersCtrl",
        templateUrl: "/FrontEnd/Markers/markers.html"
    });

    $routeProvider.when("/contacts", {
        controller: "contactsCtrl",
        templateUrl: "/FrontEnd/Contacts/contacts.html"
    });
    $routeProvider.when("/settings", {

        templateUrl: "/FrontEnd/Projects/project.html"
    });
    $routeProvider.when("/404", {
        templateUrl: "/FrontEnd/Info/404.html"
    });

    $routeProvider.otherwise({ redirectTo: "/projects" });
});




