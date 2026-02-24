angular.module('admin')
    .service('torneosService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filter, pageIndex) {
                var deffered = $q.defer();
                $http.get('/torneos/obtenerListadoAdmin?filter=' + filter + '&pageIndex=' + pageIndex)
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

            var _modificar = function (instancia) {
                var deffered = $q.defer();
                $http.post('/torneos/modificar', instancia)
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
                obtenerListado: _obtenerListado,
                obtener: _obtener,
                modificar: _modificar
            }
        }]);