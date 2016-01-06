'use strict';
app.service('contactsService', ['$http', function ($http) {

    var apiUrl = 'http://localhost:54520//api';

 
    this.getContacts = function (text) {
        return $http.get(apiUrl + '/users/contacts/');
    };

}]);

