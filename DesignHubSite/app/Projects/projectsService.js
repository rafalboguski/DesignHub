'use strict';
app.service('projectsService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

    this.getProjects = function () {
        return $http.get(apiUrl + '/projects');
    };


    this.createProject = function (user) {
        return $http({
            method: 'POST',
            url: apiUrl + '/projects',
            data: {
                'Name': user.name,
                'Description': user.description
                
            },
            headers: { 'Content-Type': 'application/json' }
        });
    };

}]);

//62937

//app.service('projectsService', function ($http) {

//    var apiUrl = 'http://localhost:4567';

//    this.Search = function() {
//        return $http.get(apiUrl + '/search/' + word);
//    };
    

//    this.addFile = function(title, content, folder) {
//        return $http({
//            method: 'POST',
//            url: apiUrl + '/push/',
//            data: {
//                'filename': title,
//                'data': content,
//                'folder': folder
//            },
//            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
//        });
//    };

//});