angular.module('admin')
    .service('noticiasService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filter,pageIndex) {
                var deffered = $q.defer();
                $http.get('/noticias/obtenerListado?filter=' + filter + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };



            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/noticias/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _modificar = function (instancia) {
                var deffered = $q.defer();
                $http.post('/noticias/modificar', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };


            var _obtenerDestacadas = function () {
                var deffered = $q.defer();
                $http.get('/noticias/obtenerDestacadas?')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };


            return {
                obtenerListado: _obtenerListado,
                obtener: _obtener,
                obtenerDestacadas: _obtenerDestacadas,
                modificar: _modificar
            }



        }]);