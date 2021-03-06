﻿'use strict';
app.controller('indexController', ['$scope', '$location', function ($scope, $location) {

    $scope.user_logged = false;
    $scope.current_page = 'projects';
    $scope.project_name = '';
    $scope.project_id = '';
    $scope.project = [];

    $scope.init = function (isLogged, userId) {
        
        
        if (isLogged === 'True') {
            $scope.user_logged = true;
            $scope.loggedUserId = userId;
        }



    };

    // this is for selecting active accordion in sidebar
    $scope.$on('$routeChangeSuccess', function (e, current, pre) {
        
        $scope.current_page = $location.url();
        var sel = '#a' + $scope.current_page
        sel = sel.replace("/", "");

        $scope.sel = sel;

        if (sel.indexOf('#aproject/') > -1) {
            if (!$('#aprojects').hasClass("active")) {
                $('#aprojects').trigger('click');
            }
        }


        if (!$(sel).hasClass("active")) {
            $(sel).trigger('click');
        }
       

    });

    // accordion don't work as <a> so this is necessary
    $scope.href = function (route) {
       
        $location.path(route);
        
    };


}]);
