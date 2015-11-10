'use strict';
app.controller('indexController', ['$scope', '$location', function ($scope, $location) {

    $scope.user_logged = false;
    $scope.current_page = 'Projects';

    $scope.init = function (isLogged) {

        if (isLogged === 'True')
            $scope.user_logged = true;

        // $("#a" + $scope.current_page).trigger("click");

        var val = '#a' + $scope.current_page;


        $('#aProjects').trigger('click');




    };

    $scope.href = function (route) {
        $location.path(route);
    };


}]);


$("#MenuList").collapse();

$("#MenuList").bind("open", function (e, section) {
    console.log(section, " is open");
});
