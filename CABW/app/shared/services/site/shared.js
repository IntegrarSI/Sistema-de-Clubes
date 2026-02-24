angular.module('site')
    .service('sharedService', function ($http, $q) {

        this.send = function (instance) {
            return $http.post('/shared/sendMail', instance);
        };

    });
