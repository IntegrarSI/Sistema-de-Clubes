angular.module('site')
    .service('torneosService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filter, pageIndex) {
                var deffered = $q.defer();
                $http.get('/torneos/obtenerListado')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _proximosEventos = function () {
                var deffered = $q.defer();
                $http.get('/torneos/proximosEventos')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _ultimosEventos = function () {
                var deffered = $q.defer();
                $http.get('/torneos/ultimosEventos')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/torneos/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            return {
                obtenerListado: _obtenerListado,
                proximosEventos: _proximosEventos,
                ultimosEventos: _ultimosEventos,
                obtener: _obtener
            }
        }]);