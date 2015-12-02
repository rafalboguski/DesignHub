'use strict';
app.controller('nodesCtrl', ['$scope', '$routeParams', '$location', 'Upload', '$timeout', 'projectsService', 'nodesService',
    function ($scope, $routeParams, $location, Upload, $timeout, projectsService, nodesService) {


        $scope.projectId = $routeParams.projectId;

        $scope.nodes;

        $scope.selectManyNodes = false;
        $scope.selectedNodesId = [-1];


        $scope.getNodes = function () {

            nodesService.getNodes($scope.projectId).then(function (res) {
                // czyste dane a api
                $scope.nodes = res.data;

                // opakowanie dla JointJs, potrzebne do wykresu svg
                console.log('Create nodes');
                angular.forEach($scope.nodes, function (node) {
                    console.log('Node ' + node.id);
                   
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
                    console.log(node.rect);
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



        $scope.init = function () {

            $scope.getNodes();


        }



        // save graph and nodes
        $scope.saveAll = function () {

            angular.forEach($scope.nodes, function (node) {

                nodesService.saveNode(node.rect.attributes.nodeId, {
                    position: {
                        x: node.rect.attributes.position.x,
                        y: node.rect.attributes.position.y
                    }
                }).then(function (results) {

                }, function (error) {
                    alert(error.data.message);
                });
            });

            $scope.drawGraph();
            alert("Saved");


        }

        $scope.drawGraph = function () {



            var graph = new joint.dia.Graph;

            var paper = new joint.dia.Paper({
                el: $('#myholder'),
                width: 1600,
                height: 1200,
                model: graph,
                gridSize: 1
            });







            //var link = new joint.dia.Link({
            //    source: { id: rect.id },
            //    target: { id: rect2.id },
            //    attrs: {
            //        '.connection': { stroke: '#B2B2B2', 'stroke-width': 3 },
            //        '.marker-source': { fill: '#B2B2B2', stroke: '#B2B2B2', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 },
            //        '.marker-target': { fill: '#B2B2B2', stroke: '#B2B2B2', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 }
            //    }
            //});

            graph.clear();
            console.log('Draw nodes');
            angular.forEach($scope.nodes, function (value) {

                graph.addCells([value.rect]);
            });



            //graph.addCells([rect, rect2, link]);

            graph.on('all', function (eventName, cell) {



                if (eventName == 'change:position') {

                    // Wybierz jeden
                    if ($scope.selectManyNodes == false) {
                        $('.Rect' + $scope.selectedNodesId[0]).attr('fill', '#B2B2B2');
                        $scope.selectedNodesId = [];
                        $scope.selectedNodesId.push(cell.attributes.nodeId);
                        $('.Rect' + $scope.selectedNodesId[0]).attr('fill', 'blue');
                    }
                    else {


                    }
                    // Wybierz kilka

                }
            });

        }




    }]);





