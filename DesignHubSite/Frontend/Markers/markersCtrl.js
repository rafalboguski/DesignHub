'use strict';
app.controller('markersCtrl', ['$scope', '$route', '$routeParams', '$location', 'projectsService', 'nodesService',
    function ($scope, $route, $routeParams, $location, projectsService, nodesService) {



        $scope.nodeId;
        $scope.node;

        $scope.image;



        $scope.click = function () {
            $scope.resizeImage();
        }


        $scope.init = function () {

            $("#image").load(function () {
                $scope.resizeImage();
            });

            document.getElementById("markers-window").addEventListener("wheel", function myFunction() {
                $scope.resizeImage();
            });
            document.getElementById("markers-window").addEventListener("onkeydown", function myFunction() {
                $scope.resizeImage();
            });

            


            $scope.nodeId = $routeParams.nodeId;
            $scope.getNode($scope.nodeId);
            $scope.getNodeImage($scope.nodeId);

        }

        $scope.resizeImage = function () {

            // fun starts here ... T__T
            var imageW = $('#image').width();

            var imageH = $('#image').height();
            var ratio = parseFloat(imageW) / parseFloat(imageH);

            if (ratio > 1) {
                // wide
                var newW = $('#image-container').width();
                var newH = $('#image-container').width() / ratio

                if (newH > $('#image-container').height()) {

                    newH = $('#image-container').height();
                    newW = newH * ratio;
                }
                $('#image').width(newW);
                $('#image').height(newH);
            }
            else {
                //tall
                var newH = $('#image-container').height();
                var newW = newH * ratio;
               
                console.log($('#image-container').width());
                if (newW > $('#image-container').width()) {
                    newW = $('#image-container').width;
                    newH = newW/ratio;
                }
                $('#image').width(newW);
                $('#image').height(newH);
            }
        }

        $scope.getNode = function (id) {
            nodesService.getNode(id).then(function (results) {

                $scope.node = results.data;

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.getNodeImage = function (id) {
            nodesService.getNodeImage(id).then(function (results) {

                $scope.image = (results.data != "null") ? results.data.substring(1, results.data.length - 1) : null;

            }, function (error) {
                alert(error.data.message);
            });
        }



    }]);





