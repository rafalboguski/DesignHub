﻿'use strict';
app.controller('markersCtrl', ['$scope', '$route', '$routeParams', '$location', 'markersService', 'nodesService',
    function ($scope, $route, $routeParams, $location, markersService, nodesService) {



        $scope.nodeId;
        $scope.node;

        $scope.image;

        $scope.newMarker = { width: 50, height: 50 };
        $scope.selectedMarker;
        $scope.markers;

        $scope.click = function (click) {
            console.log('function $scope.click ');
            $scope.resizeImage();
            //alert($("#image").offset().left);


            $scope.newMarker.x = click.offsetX / $("#image").width();
            $scope.newMarker.y = click.offsetY / $("#image").height();




            //$('body').arrive('.lean-overlay', function () {
            //    alert('sdf');
            //    //$('.lean-overlay').css('opacity', '0');
            //});


        }

        $scope.markerClick = function (id) {
            console.log('function $scope.markerClick ');
            $scope.selectedMarker = _.find($scope.markers, function (rw) { return rw.id == id });
            console.log($scope.selectedMarker);


            $(".tag").each(function (index) {


                $(this).css('background-color', 'transparent');

            });

            $('#tag' + id).css('background-color', 'rgb(0, 255, 231)');
            $('#tag' + id).css('opacity', 0.9);

        }

        $scope.showMarkers = true;
        $scope.markersOpacity = 100;

        $scope.imageContainerHeight = 700;
        $scope.imageContainerBiger = function () {
            console.log('function $scope.imageContainerBiger ');
            $scope.imageContainerHeight += 90;
            $scope.resizeImage();


        }
        $scope.imageContainerSmaller = function () {
            console.log('function $scope.imageContainerSmaller ');
            $scope.imageContainerHeight -= 90;
            $scope.resizeImage();

        }


        $scope.init = function () {
            console.log('function $scope.init ');
            $("#image").load(function () {
                $scope.resizeImage();
                Materialize.fadeInImage('#image');
                Materialize.fadeInImage('.tag');
            });

            document.getElementById("markers-window").addEventListener("wheel", function myFunction() {
                $scope.resizeImage();
            });
            document.getElementById("markers-window").addEventListener("onkeydown", function myFunction() {
                $scope.resizeImage();
            });

            $(document).arrive(".lean-overlay", function () {
                // 'this' refers to the newly created element
                console.log('modal fade');

                $(this).css('background-color', 'hotpink');

                $(this).click(function () {
                    $(".tag").each(function (index) {

                        $(this).css('background-color', 'hotpink');

                    });
                });

            });


            var slider = document.getElementById('slider');
            noUiSlider.create(slider, {
                start: 100,
                connect: "lower",
                step: 1,
                range: {
                    'min': 10,
                    'max': 100
                },
                format: wNumb({
                    decimals: 0
                })
            });
            slider.noUiSlider.on('slide', function () {
                $('.tag').css('opacity', slider.noUiSlider.get() / 100);

            });

            $scope.nodeId = $routeParams.nodeId;
            $scope.getNode($scope.nodeId);
            $scope.getNodeImage($scope.nodeId);

            $scope.getMarkers();

        }

        $scope.resizeImage = function () {

            console.log('function $scope.resizeImage');

            // fun starts here ... T__T
            var imageW = $('#image').width();

            var imageH = $('#image').height();
            var ratio = parseFloat(imageW) / parseFloat(imageH);

            if (ratio > 1) {
                // wide
                var newW = $('#image-container').width();
                var newH = $('#image-container').width() / ratio

                if (newH > $scope.imageContainerHeight) {

                    newH = $scope.imageContainerHeight;
                    newW = newH * ratio;
                }
                $('#image').width(newW);
                $('#image').height(newH);
            }
            else {
                //tall
                var newH = $scope.imageContainerHeight;
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
            console.log('function $scope.resizeTags');
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


        $scope.createMarker = function (updateId) {
            console.log('function $scope.createMarker');
            $scope.newMarker.nodeId = $scope.nodeId;
            $scope.newMarker.Id = updateId;
            markersService.createMarker($scope.newMarker).then(function (results) {

                Materialize.toast('Saved', 2500);
                $('.toast').addClass('green');
                $scope.newMarker.text = '';
                $('#AddMarkerModal').closeModal();
                $scope.getMarkers();
                $scope.newMarker.Id = null;


            }, function (error) {
                Materialize.toast('Error: ' + error.data.message, 1000);
                $('.toast').addClass('red');
            });

        }

        $scope.initModals = function () {
            console.log('function $scope.initModals');
            $('.tag').leanModal(); // Initialize the modals
            $('.tooltipped').tooltip({ delay: 50 });
        }

        $scope.getNode = function (id) {
            console.log('function $scope.getNode ' + id);
            nodesService.getNode(id).then(function (results) {

                $scope.node = results.data;

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.getMarkers = function () {
            console.log('function $scope.getMarkers ');
            markersService.getMarkers($scope.nodeId).then(function (results) {

                $scope.markers = results.data;

                angular.forEach($scope.markers, function (value, key) {
                    console.log(value);
                    if (value.opinions.length == 1) {
                        value.text = value.opinions[0].opinion;
                    }
                    else {
                        value.text = value.opinions.length + ' opinions, click to see them';
                    }


                });








                Materialize.toast('Data loadded', 1500);






            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.getNodeImage = function (id) {
            console.log('function $scope.getNodeImage ' + id);
            nodesService.getNodeImage(id).then(function (results) {

                $scope.image = (results.data != "null") ? results.data.substring(1, results.data.length - 1) : null;

            }, function (error) {
                alert(error.data.message);
            });
        }


        // project

        $scope.likeNode = function () {
            var id = $scope.nodeId;
            console.log('function $scope.likeNode ' + id);
            nodesService.like(id).then(function (results) {
                $scope.getNode($scope.nodeId);
            }, function (error) {
                alert(error.data.message);
            });
        };
        $scope.dislikeNode = function () {
            var id = $scope.nodeId;
            console.log('function $scope.dislikeNode ' + id);
            nodesService.dislike(id).then(function (results) {
                console.log(results);
                $scope.getNode($scope.nodeId);
            }, function (error) {
                alert(error.data.message);
            });
        };

        $scope.acceptNode = function () {
            var id = $scope.nodeId;
            console.log('function $scope.acceptNode ' + id);
            nodesService.accept(id).then(function (results) {
                $scope.node.accepted = !$scope.node.accepted;
                materialize.toast('Node Accepted', 2000);
            }, function (error) {
                alert(error.data.message);
            });
        };

        $scope.rejectNode = function () {
            var id = $scope.nodeId;
            console.log('function $scope.rejectNode ' + id);
            nodesService.reject(id).then(function (results) {
                $scope.node.rejected = !$scope.node.rejected;
                console.log($scope.node.rejected);
                materialize.toast('Node Accepted', 2000);
            }, function (error) {
                alert(error.data.message);
            });
        };


        //


    }]);






