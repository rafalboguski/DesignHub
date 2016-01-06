'use strict';
app.controller('contactsCtrl', ['$scope', '$route', 'contactsService',
    function ($scope, $route, contactsService) {


        $scope.init = function () {

            $scope.getContacts();

        }

        $scope.getContacts = function () {


            contactsService.getContacts().then(function (results) {
                console.log(results)
                $scope.contacts = results.data;
            }, function (error) { alert('connection error'); console.log(error); });

        }

        $scope.goToProject = function (id) {
            $scope.$parent.href('/project/' + id);
        }

        $scope.search = function (user) {
            if (
                   user.Item1.Name.indexOf($scope.searchText) > -1
                || user.Item1.Mail.indexOf($scope.searchText) > -1
                || user.Item1.Phone.indexOf($scope.searchText) > -1
                || $scope.searchText == null
                || $scope.searchText == ''
            )
            {
                return true;
            }
            return false;
        };





    }]
);






