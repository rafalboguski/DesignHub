'use strict';
app.controller('indexController', ['$scope', function ($scope) {

    $scope.user_logged = false;
    $scope.current_page = "no set";

    $scope.init = function (isLogged) {

        if (isLogged === 'True')
            $scope.user_logged = true;

    };
    $scope.updateLocation = function (page) {

        $scope.current_page = page;

    };


}]);