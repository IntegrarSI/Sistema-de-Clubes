angular.module('admin')
    .controller('password_editCtrl',
        ['$scope', '$state', 'loginService', 'notificationService'
        , function ($scope, $state, loginService, notificationService) {

            $scope.instancia = {
                PasswordActual: '',
                PasswordNueva: '',
                ConfirmacionPassword: ''
            }

            $scope.submitLoading = false;
            $scope.submit = function () {
                $scope.submitLoading = true;
                loginService.cambiarPassword($scope.instancia).then(
                     function (result) {
                         if (result.data.success == 'true') {
                             notificationService.show('green', 'Perfecto!', 'La modificación fue realizada correctamente');
                             $state.go('dashboard');
                         }
                         else
                             notificationService.show('red', 'Ups!', result.data.message);

                         $scope.submitLoading = false;
                     }
                );
            }

    }]);
