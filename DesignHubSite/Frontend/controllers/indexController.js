﻿'use strict';
app.controller('indexController', ['$scope', function ($scope) {

    $scope.user_logged = false;
    $scope.current_page = "projects";

    $scope.init = function (isLogged) {

        if (isLogged === 'True')
            $scope.user_logged = true;

    };
   

}]);