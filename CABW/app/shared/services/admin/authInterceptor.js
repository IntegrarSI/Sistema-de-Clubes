'use strict';

angular.module('admin').factory('authInterceptor',
    ['$q', '$location', 
    function ($q, $location) {

    var authInterceptorFactory = {};

    var _request = function (config) {

        return config;
    }

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
            //window.location.href = '/Admin/#/login/true';
            var hash = window.location.hash.replace('#/', '');
            while (hash.indexOf('/') > -1)
                hash = hash.replace('/', '__');

            window.location.href = '/Admin/#/login/' + hash;
        }
        return $q.reject(rejection);
    }

    authInterceptorFactory.request = _request;
    authInterceptorFactory.responseError = _responseError;

    return authInterceptorFactory;
}]);
