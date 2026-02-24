angular.module('admin')
    .service('asociacionesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListaAutocompletar = function (filter) {
                var deffered = $q.defer();
                $http.get('/asociaciones/obtenerListaAutocompletar?filter=' + filter)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerListado = function (filter, idFederacion, pageIndex) {
                var deffered = $q.defer();
                $http.get('/asociaciones/obtenerListado?filter=' + filter + '&idFederacion=' + idFederacion + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerLista = function (idFederacion) {
                var deffered = $q.defer();
                $http.post('/federaciones/obtenerAsociaciones?id=' + idFederacion)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerListaPorUsuario = function (filter) {
                var deffered = $q.defer();
                $http.post('/asociaciones/obtenerListaPorUsuario?filter='+filter)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/asociaciones/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _modificar = function (instancia) {
                var deffered = $q.defer();
                $http.post('/asociaciones/modificar', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };
            
            return {
                obtenerListaAutocompletar: _obtenerListaAutocompletar,
                obtenerListado: _obtenerListado,
                obtenerLista: _obtenerLista,
                obtenerListaPorUsuario: _obtenerListaPorUsuario,
                obtener: _obtener,
                modificar: _modificar
            }
        }]);