'use strict';
app.controller('indexController', ['$scope', '$location', function ($scope, $location) {

    $scope.user_logged = false;
    $scope.current_page = 'projects';

    $scope.init = function (isLogged) {
        
        
        if (isLogged === 'True') {
            $scope.user_logged = true;
            
        }



    };

    // this is for selecting active accordion in sidebar
    $scope.$on('$routeChangeSuccess', function (e, current, pre) {
        $scope.current_page = $location.url();
        var sel = '#a' + $scope.current_page
        sel = sel.replace("/", "");

        $scope.sel = sel;

        if (!$(sel).hasClass("active")) {
            $(sel).trigger('click');
        }

    });

    // accordion don't work as <a> so this is necessary
    $scope.href = function (route) {
        $location.path(route);
        
    };


}]);
