angular.module('admin')
    .service('federacionesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filter, pageIndex) {
                var deffered = $q.defer();
                $http.get('/federaciones/obtenerListado?filter=' + filter + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerLista = function () {
                var deffered = $q.defer();
                $http.post('/federaciones/obtenerListaPorUsuario')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerListaCompleta = function () {
                var deffered = $q.defer();
                $http.post('/federaciones/obtenerLista')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerListaAutocompletar = function (filter) {
                var deffered = $q.defer();
                $http.get('/federaciones/obtenerListaAutocompletar?filter=' + filter)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/federaciones/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _modificar = function (instancia) {
                var deffered = $q.defer();
                $http.post('/federaciones/modificar', instancia)
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
                obtenerListaCompleta: _obtenerListaCompleta,
                obtener: _obtener,
                modificar: _modificar
            }
        }]);