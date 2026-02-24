angular.module('admin')
    .service('clubesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListaAutocompletar = function (filter) {
                var deffered = $q.defer();
                $http.get('/clubes/obtenerListaAutocompletar?filter=' + filter)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerListado = function (filter, idAsociacion, pageIndex) {
                var deffered = $q.defer();
                $http.get('/clubes/obtenerListado?filter=' + filter + '&idAsociacion=' + idAsociacion + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerLista = function (idAsociacion) {
                var deffered = $q.defer();
                $http.post('/federaciones/obtenerClubes?id=' + idAsociacion)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/clubes/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _modificar = function (instancia) {
                var deffered = $q.defer();
                $http.post('/clubes/modificar', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };
            
            return {
                obtenerListaAutocompletar: _obtenerListaAutocompletar,
                obtenerListado: _obtenerListado,
                obtenerLista: _obtenerLista,
                obtener: _obtener,
                modificar: _modificar
            }
        }]);