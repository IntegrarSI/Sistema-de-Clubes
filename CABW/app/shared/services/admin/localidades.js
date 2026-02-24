angular.module('admin')
    .service('localidadesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListaAutocompletar = function (filter) {
                var deffered = $q.defer();
                $http.get('/localidades/obtenerListaAutocompletar?filter=' + filter)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };
            
            return {
                obtenerListaAutocompletar: _obtenerListaAutocompletar
            }
        }]);