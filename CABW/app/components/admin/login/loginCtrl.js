
angular.module('admin')
    .controller('loginCtrl',
        ['$scope', '$state', '$location', '$stateParams', '$window', 'loginService', 'auth', 'focus'
        , function ($scope, $state, $location, $stateParams, $window, loginService, auth, focus) {

            auth.clean();

            $scope.expired = $stateParams.returnUrl != null && $stateParams.returnUrl !="recuperar" ? true : false;

            $scope.ticket = [];
            $scope.isLoading = false;
            $scope.submit = function () {
                $scope.isLoading = true;
                loginService
                   .login($scope.dataLogin)
                   .success(function (result) {
                       $scope.isLoading = false;
                       $scope.ticket = result;
                       if ($scope.ticket.nombreUsuario != null && $scope.ticket.passwordIncorrecta == false && $scope.ticket.activo == true) {
                           $window.localStorage.ticket = JSON.stringify(result);

                           if ($scope.dataLogin.password == $scope.dataLogin.nombreUsuario.toLowerCase() + '1234')
                               $state.go('password_force_edit');
                           else {
                               if ($stateParams.returnUrl != null) {
                                   var hash = $stateParams.returnUrl;
                                   while (hash.indexOf('__') > -1)
                                       hash = hash.replace('__', '/');

                                   window.location = '/admin/dashboard/#/' + hash;
                               }
                               else
                                   window.location = '/admin/dashboard/#/dashboard';
                           }
                       }
                   })
                   .error(function (err) {
                       $scope.isLoading = false;
                   });

                return false;
            }

            /*$scope.dataLogin = {
                nombreUsuario: 'ezvi',
                password: 'ezequiel'
            }
            //borrar
            $scope.submit();*/

            $scope.dataLogin = {
                nombreUsuario: '',
                password: ''
            }

            focus('nombreUsuario');
        }]);
