angular.module('admin')
    .controller('password_force_editCtrl',
        ['$scope', '$location', 'loginService', 'notificationService', 'auth'
        , function ($scope, $location, loginService, notificationService, auth) {

            $scope.instancia = {
                PasswordActual: '',
                PasswordNueva: '',
                ConfirmacionPassword: ''
            }

            $scope.ticket = auth.me();
            $scope.instancia.PasswordActual = $scope.ticket.nombreUsuario.toLowerCase() + '1234';

            $scope.submitLoading = false;
            $scope.submit = function () {
                $scope.submitLoading = true;
                loginService.cambiarPassword($scope.instancia).then(
                     function (result) {
                         if (result.data.success == 'true') {
                             //notificationService.show('green', 'Perfecto!', 'La modificación fue realizada correctamente');
                             window.location = '/admin/dashboard/#/dashboard';
                         }
                         else
                             notificationService.show('red', 'Ups!', result.data.message);

                         $scope.submitLoading = false;
                     }
                );

                return false;
            }

    }]);
