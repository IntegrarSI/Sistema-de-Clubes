angular.module('admin')
    .service('db', function ($http, $q) {

        var self = this;

        self.loadPromise = null;

        self.federaciones = function () {
            return $http.post('/Federaciones/obtenerFederaciones');
        };

        self.asociaciones = function (id) {
            return $http.post('/Federaciones/obtenerAsociaciones/' + id);
        };

        self.clubes = function (id) {
            return $http.post('/Federaciones/obtenerClubes/' + id);
        };
    });
