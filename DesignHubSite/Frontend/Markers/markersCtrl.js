'use strict';
app.controller('markersCtrl', ['$scope', '$route', '$routeParams', '$location', 'markersService', 'nodesService',
    function ($scope, $route, $routeParams, $location, markersService, nodesService) {



        $scope.nodeId;
        $scope.node;

        $scope.image;

        $scope.newMarker = { width: 50, height: 50 };

        $scope.markers;

        $scope.click = function (click) {
            $scope.resizeImage();
            //alert($("#image").offset().left);


            $scope.newMarker.x = click.offsetX / $("#image").width();
            $scope.newMarker.y = click.offsetY / $("#image").height();


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

            $scope.getMarkers();

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
                    newH = newW / ratio;
                }
                $('#image').width(newW);
                $('#image').height(newH);
            }
            $scope.resizeTags();
        }


        $scope.resizeTags = function () {
            var imagePosX = $("#image").position().left;
            var imageW = $("#image").width();

            var imagePosY = $("#image").position().top;
            var imageH = $("#image").height();

            //console.log("image x: " + imagePosX);
            //console.log("image W: " + imageW);

            $(".tag").each(function (index) {
                var left = parseFloat($(this).attr("left"));
                var width = parseFloat($(this).attr("width"));
                $(this).css({ left: (imagePosX + (imageW * left) - width / 2) });


                var top = parseFloat($(this).attr("top"));
                var height = parseFloat($(this).attr("height"));
                $(this).css({ top: (imagePosY + (imageH * top) - height / 2) });


            });
        }


        $scope.createMarker = function () {

            $scope.newMarker.nodeId = $scope.nodeId;

            markersService.createMarker($scope.newMarker).then(function (results) {

                Materialize.toast('Saved', 500);
                $('.toast').addClass('green');
                newMarker.text = '';
                $('#AddMarkerModal').closeModal();


            }, function (error) {
                Materialize.toast('Error: '+error.data.message, 1000);
                $('.toast').addClass('red');
            });

        }

        $scope.getNode = function (id) {
            nodesService.getNode(id).then(function (results) {

                $scope.node = results.data;

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.getMarkers = function () {
            markersService.getMarkers($scope.nodeId).then(function (results) {

                $scope.markers = results.data;

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





