angular.module('admin')
    .service('rolesService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filter, pageIndex) {
                var deffered = $q.defer();
                $http.get('/roles/obtenerListado?filter=' + filter + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerLista = function (filter) {
                var deffered = $q.defer();
                var url = '/roles/obtenerLista';

                if (filter == 'federacion')
                    url = '/roles/obtenerFederacion';
                $http.get(url)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };
          
            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/roles/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            return {
                obtenerListado: _obtenerListado,
                obtenerLista: _obtenerLista,
                obtener: _obtener
            }
        }]);