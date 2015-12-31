'use strict';
app.service('nodesService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getNodes = function (projectId) {
        return $http.get(apiUrl + '/nodes/project/' + projectId);
    };

    this.getNode = function (id) {
        return $http.get(apiUrl + '/nodes/' + id);
    };

    this.getNodeImage = function (id) {
        return $http.get(apiUrl + '/nodes/' + id + '/image');
    };

    this.setNodeHead = function (id) {
        return $http({
            method: 'POST',
            url: apiUrl + '/nodes/' + id + '/head',
            headers: { 'Content-Type': 'application/json' }
        });
    }

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

    this.createNode = function (nodeDto) {
        return $http({
            method: 'POST',
            url: apiUrl + '/nodes',
            data: {
                'ChangeInfo': nodeDto.ChangeInfo,
                'Image': nodeDto.Image,
                'Description': nodeDto.description,
                'ParentsId': nodeDto.ParentsId,
                'ProjectId': nodeDto.ProjectId

            },
            headers: { 'Content-Type': 'application/json' }
        });
    };

    // wyslanie obrazu tylko do istniejacego noda, tylko do tesow
    this.uploadImageAdress = function (nodeId) {
        return apiUrl + '/Nodes/' + nodeId + '/image';
    };


    this.like = function (id) {
        return $http.post(apiUrl + '/nodes/' + id + '/like');
    };
    this.dislike = function (id) {
        console.log('nodesService dislike '+id);
        return $http.post(apiUrl + '/nodes/' + id + '/dislike');
    };

    this.accept = function (id) {
        return $http.post(apiUrl + '/nodes/' + id + '/accept');
    };

    this.reject = function (id) {
        return $http.post(apiUrl + '/nodes/' + id + '/reject');
    };




}]);

