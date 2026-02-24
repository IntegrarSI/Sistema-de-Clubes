angular.module('admin')
    .service('loginService',
        ['$http', '$q',
        function ($http, $q) {

            var _login = function (data) {
                return $http.post('/auth/Login', data);
            };

            var _recuperarPassword = function (data) {
                return $http.post('/auth/RecuperarPassword', data);
            };

            var _cambiarPassword = function (data) {
                return $http.post('/auth/CambiarPassword', data);
            };

            return {
                login: _login
                , cambiarPassword: _cambiarPassword
                , recuperarPassword: _recuperarPassword
            }
        }]);