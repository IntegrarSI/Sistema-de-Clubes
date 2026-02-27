angular.module('site')
    .service('noticiasService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filter, idAsociacion, pageIndex) {
                var deffered = $q.defer();
                $http.get('/noticias/obtenerListado?filter=' + filter + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerTodas = function (pageIndex, pageSize) {
                var deffered = $q.defer();
                $http.get('/noticias/obtenerTodas?pageIndex=' + pageIndex + '&pageSize=' + pageSize)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };



            var _obtenerNoticia = function (id) {
                var deffered = $q.defer();
                $http.post('/noticias/obtenerNoticia?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerUrl = function (id) {
                var deffered = $q.defer();
                $http.post('/noticias/obtenerUrl?')
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
                obtenerNoticia: _obtenerNoticia,
                obtenerDestacadas: _obtenerDestacadas,
                obtenerTodas: _obtenerTodas,
                obtenerUrl: _obtenerUrl
          
            }



        }]);