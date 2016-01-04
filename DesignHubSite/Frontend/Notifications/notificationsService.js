'use strict';
app.service('notificationsService', ['$http', function ($http) {

	var apiUrl = 'http://localhost:54520//api';

	this.get = function (id) {
		return $http.get(apiUrl + '/notifications/' + id);
	};


	this.getAll = function (id) {
	    return $http.get(apiUrl + '/notifications/project/' + id);
	};

	this.seen = function (id) {
		return $http.post(apiUrl + '/notifications/' + id + '/seen');
	};

}]);

