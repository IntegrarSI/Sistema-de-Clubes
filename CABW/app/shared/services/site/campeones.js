angular.module('site')
    .service('campeonesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (pageIndex, pageSize, pTorneo) {
                var deffered = $q.defer();
                $http.get('/campeones/obtenerListado?pageIndex=' + pageIndex + '&pageSize=' + pageSize + '&pTorneo=' + pTorneo)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerUltimos = function () {
                var deffered = $q.defer();
                $http.get('/campeones/obtenerUltimos?')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            
            return {
                obtenerListado: _obtenerListado,
                obtenerUltimos: _obtenerUltimos
            }



        }]);