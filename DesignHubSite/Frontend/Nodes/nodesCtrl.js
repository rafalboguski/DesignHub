'use strict';
app.controller('nodesCtrl', ['$scope', '$routeParams', '$location', 'Upload', '$timeout', 'projectsService',
    function ($scope, $routeParams, $location, Upload, $timeout, projectsService) {


        $scope.projectId = $routeParams.projectId;



        $scope.getProject = function () {
            projectsService.getProject($scope.projectId).then(function (results) {

                $scope.project = results.data;



            }, function (error) {
                alert(error.data.message);
            });
        }



        $scope.init = function () {

            $scope.drawGraph();

        }


        $scope.drawGraph = function () {


            joint.shapes.basic.Rect = joint.shapes.basic.Generic.extend({
                markup: '<g class="rotatable"><g class="scalable"><rect/></g><text/></g>',

                defaults: joint.util.deepSupplement({
                    type: 'basic.Rect',
                    attrs: {
                        'rect': { fill: 'white', stroke: 'black', 'follow-scale': true, width: 80, height: 40 },
                        'text': { 'font-size': 14, 'ref-x': .5, 'ref-y': .5, ref: 'rect', 'y-alignment': 'middle', 'x-alignment': 'middle' }
                    }
                }, joint.shapes.basic.Generic.prototype.defaults)
            });

            var graph = new joint.dia.Graph;

            var paper = new joint.dia.Paper({
                el: $('#myholder'),
                width: 1600,
                height: 1200,
                model: graph,
                gridSize: 1
            });

            var rect = new joint.shapes.basic.Rect({
                position: { x: 100, y: 30 },
                size: { width: 140, height: 40 },
                attrs: {
                    
                    rect: { fill: '#B2B2B2', rx: 5, ry: 5, 'stroke-width': 0, stroke: 'black'},
                    text: {
                        onclick: "alert('fag');",
                        text: 'my label', fill: 'black',
                        'font-size': 22, 'font-weight': 'bold', 'font-variant': 'small-caps', 'text-transform': 'capitalize'
                    }

                }
            });



            var rect2 = rect.clone();
            rect2.translate(300);

            var link = new joint.dia.Link({
                source: { id: rect.id },
                target: { id: rect2.id },
                attrs: {
                    '.connection': { stroke: '#B2B2B2', 'stroke-width': 3 },
                    '.marker-source': { fill: '#B2B2B2', stroke: '#B2B2B2', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 },
                    '.marker-target': { fill: '#B2B2B2', stroke: '#B2B2B2', d: 'M 10 0 L 0 5 L 10 10 z', 'stroke-width': 5 }
                }
            });

         

            graph.addCells([rect, rect2, link]);

            graph.on('all', function (eventName, cell) {
                if (eventName != 'change:position')
                    console.log(arguments);
            });

        }




    }]);





