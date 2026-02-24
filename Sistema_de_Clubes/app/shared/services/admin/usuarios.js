angular.module('admin')
    .service('usuariosService',
        ['$http', '$q', 'notificationService',
        function ($http, $q, notificationService) {

            var _obtenerListado = function (filter, pageIndex) {
                var deffered = $q.defer();
                $http.get('/usuarios/obtenerListado?filter=' + filter + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/usuarios/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };



            var _modificar = function (instancia) {
                var deffered = $q.defer();
                $http.post('/usuarios/modificar', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _resetPassword = function (id) {
                var deffered = $q.defer();
                $http.post('/usuarios/resetPassword?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };


            var _borrarUsuario = function (id) {
                var deffered = $q.defer();
                $http.post('/usuarios/borrarUsuario?id=' + id)
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
                resetPassword: _resetPassword,
                modificar: _modificar,
                borrarUsuario: _borrarUsuario
            }
        }]);