angular.module('site')
    .controller('contactoCtrl', ['$scope', 'sharedService', function ($scope, sharedService) {
        
        $scope.mail = {}
        $scope.mensajeEnviado = false;

        $scope.submit = function () {
            sharedService.send($scope.mail);
            $scope.mensajeEnviado = true;
            return false;
        };

    }]);
