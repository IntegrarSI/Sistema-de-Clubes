angular.module('admin')
    .controller('jugadores_detailCtrl',
        ['$scope', '$rootScope', '$state', 'focus', '$stateParams', 'notificationService', 'jugadoresService'
        , function ($scope, $rootScope, $state, focus, $stateParams, notificationService, jugadoresService) {
                 
            $scope.init = function () {

                $scope.instancia = {};
                $scope.isLoading = false;
                
                if ($stateParams.id != '') {

                    //es una modificacion
                    $scope.isLoading = true;
                    $rootScope.loadingLoad = true;
                    jugadoresService.obtener($stateParams.id).then(
                        
                        function (result) {
                            $scope.instancia = result;
                            $scope.instancia.fechaNacimiento = stringToDate($scope.instancia.fechaNacimiento.toString());
                            if ($scope.instancia.fechaIngreso != null)
                                $scope.instancia.fechaIngreso = stringToDate($scope.instancia.fechaIngreso.toString());

                            $scope.isLoading = false;
                            $rootScope.loadingLoad = false;
                        });
                }
            }

            $scope.init();

    }]);

