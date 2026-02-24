angular.module('admin')
    .service('categoriasService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerLista = function (filter, pageIndex) {
                var deffered = $q.defer();
                $http.get('/categorias/obtenerLista')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };
            
            return {
                obtenerLista: _obtenerLista
            }
        }]);