'use strict';
app.service('markersService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getMarkers = function (nodeId) {
        return $http.get(apiUrl + '/markers/node/' + nodeId);
    };


    this.saveNode = function (id, changes) {

        // zmiana pozycji na grafie
        if (typeof (changes.position) != 'undefined') {
            return $http({
                method: 'PUT',
                url: apiUrl + '/nodes/' + id,
                data: {
                    'positionX': changes.position.x,
                    'positionY': changes.position.y

                },
                headers: { 'Content-Type': 'application/json' }
            });
        }
    };

    this.createMarker = function (dto) {
        return $http({
            method: 'POST',
            url: apiUrl + '/markers',
            data: {
                'X': dto.x,
                'Y': dto.y,
                'Width': dto.width,
                'Height': dto.height,
                'NodeId': dto.nodeId,
                'Text':dto.text

            },
            headers: { 'Content-Type': 'application/json' }
        });
    };




}]);

