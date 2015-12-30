'use strict';
app.controller('nodesCtrl', ['$scope', '$route', '$routeParams', '$location', 'Upload', '$timeout', 'projectsService', 'nodesService',
    function ($scope, $route, $routeParams, $location, Upload, $timeout, projectsService, nodesService) {


        $scope.projectId = $routeParams.projectId;

        $scope.selectedNode;
        $scope.nodes;
        var links = [];
        var linksId = {};

        $scope.selectManyNodes = false;
        $scope.selectedNodesId = [];


        $scope.getNodes = function () {

            nodesService.getNodes($scope.projectId).then(function (res) {
                // czyste dane a api
                $scope.nodes = res.data;

                // opakowanie dla JointJs, potrzebne do wykresu svg
                angular.forEach($scope.nodes, function (node) {
                    node.changeInfo = node.changeInfo.split("\n");

                    var title = '';
                    if (node.head == true)
                        title = 'Head: ';
                    if (node.root == true)
                        title = 'Root: ';

                    node.rect = new joint.shapes.basic.Rect({
                        nodeId: node.id,
                        position: { x: node.positionX, y: node.positionY },
                        size: { width: 150, height: 50 },
                        attrs: {

                            rect: { class: 'Rect' + node.id, fill: '#B2B2B2', rx: 5, ry: 5, 'stroke-width': 0, stroke: 'black' },
                            text: {
                                text: title + node.id + ' ...', fill: 'black',
                                'font-size': 21, 'font-weight': 'bold'
                            }
                        }
                    });
                    linksId[node.id] = node.rect.id;
                    //node.rect.id = node.id;
                    console.log(node.rect.id);
                });

                angular.forEach($scope.nodes, function (node) {

                    // use map hash

                    if (node.parent != null) {
                        var link = new joint.dia.Link({
                            source: { id: linksId[node.id] },
                            target: { id: linksId[node.parent.id] },
                            attrs: {
                                '.connection': { stroke: '#26A69A', 'stroke-width': 3 },
                                '.marker-source': { fill: '#26A69A', stroke: '#26A69A', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 },
                                '.marker-target': { fill: '#26A69A', stroke: '#26A69A', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 }
                            }
                        });
                        links.push(link);
                    }


                });





                $scope.drawGraph();


            }, function (error) {
                alert(error.data.message);
            });

        }

        $scope.getProject = function () {
            projectsService.getProject($scope.projectId).then(function (results) {

                $scope.project = results.data;



            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.clearSelection = function () {


            $scope.selectedNode = ''
            $scope.selectedNodesId.forEach(function (element) {

                $('.Rect' + element).attr('fill', '#B2B2B2');
            });
            $scope.selectedNodesId = [];
        }


        $scope.init = function () {

            $scope.$parent.project_id = $scope.projectId;

            $(document).ready(function () {
                $('.tooltipped').tooltip({ delay: 50 });
            });

            $scope.getNodes();


        }



        $scope.createNode = function (node, file) {

            console.log('create');
            console.log($scope.selectedNodesId[0]);
            node.ParentId = $scope.selectedNodesId[0];
            node.ProjectId = $scope.projectId;
            nodesService.createNode(node).then(function (res) {



                var node = res.data;

                ////

                $scope.f = file;
                if (file && !file.$error) {
                    file.upload = Upload.upload({
                        url: nodesService.uploadImageAdress(node.id),
                        file: file
                    });

                    file.upload.then(function (response) {
                        $timeout(function () {
                            file.result = response.data;
                            $('#AddNodeModal').closeModal();
                            Materialize.toast('Node saved', 500);
                            $('.toast').addClass('red');
                            $route.reload();
                        });
                    }, function (response) {
                        if (response.status > 0)
                            $scope.errorMsg = response.status + ': ' + response.data;
                    });

                    file.upload.progress(function (evt) {
                        file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                    });
                }

                ////


            }, function (error) {
                alert(error.data.message);
            });

        }

        $scope.setNodeHead = function () {

            nodesService.setNodeHead($scope.selectedNode.id).then(function (res) {
                Materialize.toast('Saved as Head', 1500);
                $('.toast').addClass('green');
                $route.reload();
            }, function (error) {
                Materialize.toast(error.data.message, 1000);
                $('.toast').addClass('red');
            })
        }


        $scope.graphClick = function () {

            console.log($scope.selectedNodesId);

            $('g').click(function (event) {
                console.log('rect click');

            })

            console.log($scope.nodes);
            $scope.selectedNode = _.find($scope.nodes, function (n) {
                return n.id == $scope.selectedNodesId
            });

            if ($scope.selectedNode.image == undefined) {
                nodesService.getNodeImage($scope.selectedNodesId).then(function (results) {

                    $scope.selectedNode.image = (results.data != "null") ? results.data.substring(1, results.data.length - 1) : null;
                }, function (error) {
                    alert('getNodeImage' + error.data.message);
                });
            }



        }

        // save graph and nodes
        $scope.saveAll = function () {

            var i = $scope.nodes.length;
            angular.forEach($scope.nodes, function (node) {

                nodesService.saveNode(node.rect.attributes.nodeId, {
                    position: {
                        x: node.rect.attributes.position.x,
                        y: node.rect.attributes.position.y
                    }
                }).then(function (results) {

                    i--;
                    if (i <= 0) {

                        Materialize.toast('Saved', 500);
                        $('.toast').addClass('green');

                        $route.reload();
                    }

                }, function (error) {
                    alert(error.data.message);
                });
            });

        }

        var graph = new joint.dia.Graph;
        var paper;
        $scope.drawGraph = function () {


            paper = new joint.dia.Paper({
                el: $('#myholder'),
                width: 800,
                height: 500,
                model: graph,
                gridSize: 1
            });

            graph.clear();


            //var link = new joint.dia.Link({
            //    source: { id: rect.id },
            //    target: { id: rect2.id },
            //    attrs: {
            //        '.connection': { stroke: '#B2B2B2', 'stroke-width': 3 },
            //        '.marker-source': { fill: '#B2B2B2', stroke: '#B2B2B2', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 },
            //        '.marker-target': { fill: '#B2B2B2', stroke: '#B2B2B2', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 }
            //    }
            //});


            angular.forEach($scope.nodes, function (value) {
                graph.addCells([value.rect]);
            });
            graph.addCells(links);

            //graph.addCells([rect, rect2, link]);

            paper.on('cell:pointerdown',
                function (cellView, evt, x, y) {
                    var cell = cellView.model;

                    var currentId = cell.attributes.nodeId;
                    // Zaznacz jeden node
                    if ($scope.selectManyNodes == false) {
                        $('.Rect' + $scope.selectedNodesId[0]).attr('fill', '#B2B2B2');
                        $scope.selectedNodesId = [];
                        $scope.selectedNodesId.push(currentId);
                        $('.Rect' + $scope.selectedNodesId[0]).attr('fill', '#F44336');
                    }
                        // Zaznaczam kilka
                    else {
                        if ($scope.selectedNodesId.indexOf(currentId) == -1) {
                            $scope.selectedNodesId.push(currentId);

                            var foundId = $scope.selectedNodesId.indexOf(currentId);
                            $('.Rect' + $scope.selectedNodesId[foundId]).attr('fill', '#F44336');
                        }
                    }
                }
            );


        }




    }]);





