angular.module('site')
    .service('jugadoresService',
        ['$http', '$q',
        function ($http, $q) {

          

            var _obtenerPorDni = function (Dni) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtenerPorDni?dni=' + Dni)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;



            };

            var _obtenerPublico = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtenerPublico?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtenerUrl = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtenerUrl?')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            
            return {
                obtenerPorDni: _obtenerPorDni,
                obtenerPublico: _obtenerPublico,
                obtenerUrl: _obtenerUrl
            }



        }]);