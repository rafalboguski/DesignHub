
app.controller('notificationsCtrl', ['$scope', '$route', '$routeParams', 'notificationsService',
    function ($scope, $route, $routeParams, notificationsService) {


        $scope.projectId = $routeParams.projectId;


        $scope.init = function () {

            $scope.$parent.project_id = $scope.projectId;


            $scope.getNotyfications();
        }

        $scope.getNotyfications = function () {
            notificationsService.getAll($scope.projectId).then(function (results) {

                $scope.notifications = results.data;

            }, function (error) {
                alert('connection timeout');
                console.log(error);
            });
        }

        $scope.notificationSeen = function (notification) {
            notificationsService.seen(notification.id).then(function (results) {

                if (notification.visited) {
                    notification.visited = false;
                }
                else {
                    notification.visited = true;
                }


            }, function (error) {
                alert('connection timeout');
                console.log(error);
            });
        }

    }
]);





