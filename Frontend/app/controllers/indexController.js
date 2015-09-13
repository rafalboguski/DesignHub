'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.authentication = authService.authentication;



    // Horizontal menu -------------------------------
    $scope.noneStyle = false;
    $scope.bodyCon = false;

    //Toggle the styles
    $scope.toggleStyle = function () {
        //If they are true, they will become false 
        //and false will become true
        $scope.bodyCon = !$scope.bodyCon;
        $scope.noneStyle = !$scope.noneStyle;
    }
    //add class to search box
    $scope.openSearch = false;
    $scope.searchToggle = function () {

        $scope.openSearch = !$scope.openSearch;

    }
   // -----------------------------------------------


}]);