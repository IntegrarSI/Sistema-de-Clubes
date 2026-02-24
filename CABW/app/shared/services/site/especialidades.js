angular.module('site')
    .service('especialidadesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filter, pageIndex) {
                var deffered = $q.defer();
                $http.get('/torneos/obtenerListaEspecialidades')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };
            
            return {
                obtenerListado: _obtenerListado
            }
        }]);