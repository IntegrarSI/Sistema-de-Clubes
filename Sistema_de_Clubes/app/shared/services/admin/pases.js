angular.module('admin')
    .service('pasesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListadoPendientes = function (pageIndex) {
                var deffered = $q.defer();
                $http.get('/pases/obtenerListadoPendientes?pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _aprobarPase = function (id) {
                var deffered = $q.defer();
                $http.post('/pases/aprobarPase?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _revisarPase = function (id) {
                var deffered = $q.defer();
                $http.post('/pases/revisarPase?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _rechazarPase = function (id, motivos) {
                var deffered = $q.defer();
                $http.post('/pases/rechazarPase?id=' + id + '&motivos=' + motivos)
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
                obtenerListadoPendientes: _obtenerListadoPendientes,
                aprobarPase: _aprobarPase,
                revisarPase: _revisarPase,
                rechazarPase: _rechazarPase
            }
        }]);