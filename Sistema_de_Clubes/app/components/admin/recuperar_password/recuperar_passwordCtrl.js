
angular.module('admin')
    .controller('recuperar_passwordCtrl',
        ['$scope', '$location', '$stateParams', '$window', 'loginService'
        , function ($scope, $location, $stateParams, $window, loginService) {
            //auth.clean();

            $scope.dataLogin = {
                nombreUsuario: '',
                recaptchaResponse: ''
            }

            $scope.mailDestino = '';
            $scope.error;
            $scope.resultMessage = '';

            $scope.ticket = [];
            $scope.isLoading = false;
            $scope.submit = function () {
                $scope.isLoading = true;
                loginService
                   .recuperarPassword($scope.dataLogin)
                   .success(function (result) {
                       $scope.error = !(result.success == 'true');
                       $scope.resultMessage = result.message;
                       //vcRecaptchaService.reload();
                       $scope.isLoading = false;
                   })
                   .error(function (err) {
                       console.log(err);
                       //vcRecaptchaService.reload();
                       $scope.isLoading = false;
                   });

                return false;
            }

            focus('nombreUsuario');
        }]);
