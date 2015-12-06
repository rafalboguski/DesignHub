'use strict';
app.controller('nodesCtrl', ['$scope', '$route', '$routeParams', '$location', 'Upload', '$timeout', 'projectsService', 'nodesService',
    function ($scope, $route, $routeParams, $location, Upload, $timeout, projectsService, nodesService) {


        $scope.projectId = $routeParams.projectId;

        $scope.selectedNode;
        $scope.nodes;

        $scope.selectManyNodes = false;
        $scope.selectedNodesId = [];


        $scope.getNodes = function () {

            nodesService.getNodes($scope.projectId).then(function (res) {
                // czyste dane a api
                $scope.nodes = res.data;

                // opakowanie dla JointJs, potrzebne do wykresu svg
                angular.forEach($scope.nodes, function (node) {
                    node.rect = new joint.shapes.basic.Rect({
                        nodeId: node.id,
                        position: { x: node.positionX, y: node.positionY },
                        size: { width: 150, height: 50 },
                        attrs: {

                            rect: { class: 'Rect' + node.id, fill: '#B2B2B2', rx: 5, ry: 5, 'stroke-width': 0, stroke: 'black' },
                            text: {
                                text: node.changeInfo.slice(0, 11) + ' ...', fill: 'black',
                                'font-size': 21, 'font-weight': 'bold'
                            }
                        }
                    });
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

            $scope.selectedNodesId.forEach(function (element) {

                $('.Rect' + element).attr('fill', '#B2B2B2');
            });
            $scope.selectedNodesId = [];
        }


        $scope.init = function () {

            $scope.getNodes();


        }

        $scope.createNode = function (node) {

            node.ProjectId = $scope.projectId;
            nodesService.createNode(node).then(function (res) {

                $('#AddNodeModal').closeModal();

                var node = res.data;

                node.rect = new joint.shapes.basic.Rect({
                    nodeId: node.id,
                    position: { x: node.positionX, y: node.positionY },
                    size: { width: 140, height: 40 },
                    attrs: {

                        rect: { class: 'Rect' + node.id, fill: '#B2B2B2', rx: 5, ry: 5, 'stroke-width': 0, stroke: 'black' },
                        text: {
                            //onclick: "alert('fag');",
                            text: 'Id ' + node.id, fill: 'black',
                            'font-size': 22, 'font-weight': 'bold', 'font-variant': 'small-caps', 'text-transform': 'capitalize'
                        }
                    }
                });

                graph.addCells([node.rect]);
                $scope.nodes.push(node);
                alert('ok');
                node.coChangeInfo = '';

                

            }, function (error) {
                alert(error.data.message);
            });
        }

        $scope.uploadFiles = function (node, file) {


            node.ProjectId = $scope.projectId;
            nodesService.createNode(node).then(function (res) {

                $('#AddNodeModal').closeModal();

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
                            alert('ok');
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


        //$scope.uploadFiles = function (node, file) {
        //    alert('upload');
        //    $scope.f = file;
        //    if (file && !file.$error) {
        //        file.upload = Upload.upload({
        //            url: projectsService.uploadImageAdress(nodeId),
        //            file: file
        //        });

        //        file.upload.then(function (response) {
        //            $timeout(function () {
        //                alert('$timeout');
        //                file.result = response.data;
        //                node.rect = new joint.shapes.basic.Rect({
        //                    nodeId: node.id,
        //                    position: { x: node.positionX, y: node.positionY },
        //                    size: { width: 140, height: 40 },
        //                    attrs: {

        //                        rect: { class: 'Rect' + node.id, fill: '#B2B2B2', rx: 5, ry: 5, 'stroke-width': 0, stroke: 'black' },
        //                        text: {
        //                            //onclick: "alert('fag');",
        //                            text: 'Id ' + node.id, fill: 'black',
        //                            'font-size': 22, 'font-weight': 'bold', 'font-variant': 'small-caps', 'text-transform': 'capitalize'
        //                        }
        //                    }
        //                });

        //                graph.addCells([node.rect]);
        //                $scope.nodes.push(node);
        //                alert('ok');
        //                node.coChangeInfo = '';
                        

        //            });
        //        }, function (response) {
        //            if (response.status > 0) {
        //                $scope.errorMsg = response.status + ': ' + response.data;
        //                alert(response.data);
        //            }
        //        });

        //        file.upload.progress(function (evt) {
        //            file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
        //        });
        //    }
        //}

        $scope.graphClick = function () {

            console.log($scope.selectedNodesId);

            $scope.selectedNode = _.find($scope.nodes, function (n) {
                return n.id == $scope.selectedNodesId
            });

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

            //graph.addCells([rect, rect2, link]);

            graph.on('all', function (eventName, cell) {


                if (eventName == 'change:position') {
                    var currentId = cell.attributes.nodeId;
                    // Zaznacz jeden node
                    if ($scope.selectManyNodes == false) {
                        $('.Rect' + $scope.selectedNodesId[0]).attr('fill', '#B2B2B2');
                        $scope.selectedNodesId = [];
                        $scope.selectedNodesId.push(currentId);
                        $('.Rect' + $scope.selectedNodesId[0]).attr('fill', 'blue');
                    }
                        // Zaznaczam kilka
                    else {
                        if ($scope.selectedNodesId.indexOf(currentId) == -1) {
                            $scope.selectedNodesId.push(currentId);

                            var foundId = $scope.selectedNodesId.indexOf(currentId);
                            $('.Rect' + $scope.selectedNodesId[foundId]).attr('fill', 'blue');
                        }
                    }
                }
            });

        }




    }]);





