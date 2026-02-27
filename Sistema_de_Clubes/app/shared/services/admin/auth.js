angular.module('admin')
	.service('auth',
        ['$http', '$window', '$cookieStore', '$q', 'ROOT_SERVER_CONTROLLERS',
        function ($http, $window, $cookieStore, $q, baseUrl) {

            var self = this;

            self.init = function () {
                var token = $cookieStore.get('_authtoken');
                if (token == null || $window.localStorage.ticket == null)
                    if (window.location.pathname != '/admin/#/Login')
                        window.location.href = '/admin/#/login';
                     
            };

            self.logout = function () {
                $cookieStore.remove('_authtoken');
                $window.localStorage.ticket = null;
                window.location.href = '/admin/#/login';
            };

            self.clean = function () {
                $cookieStore.remove('_authtoken');
                $window.localStorage.ticket = null;
            };

            self.isLoggedIn = function () {
                return $window.localStorage.ticket !== null;
            };

            self.me = function () {
                return JSON.parse($window.localStorage.ticket);
            };

          

        }]);