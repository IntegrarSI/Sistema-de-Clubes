angular.module('site')
    .controller('calendario_resultadosCtrl',
    ['$scope', '$rootScope', '$stateParams', 'torneosService', 'especialidadesService'
    , function ($scope, $rootScope, $stateParams, torneosService, especialidadesService) {
        
        $scope.fases = [];
        $scope.isLoading = true;

        $scope.init = function () {
            if ($stateParams.id != '') {
                $rootScope.loadingLoad = true;
                torneosService.obtener($stateParams.id).then(
                    function (result) {
                        $scope.isLoading = false;
                        $scope.instancia = result;
                        if ($scope.instancia.resultadosDescripcion != null)
                            $scope.instancia.resultadosDescripcion = $scope.instancia.resultadosDescripcion.replace('\n', '<br/><br/>');
                        $scope.instancia.fechaDesde = stringToDate($scope.instancia.fechaDesde.toString());
                        $scope.instancia.fechaHasta = stringToDate($scope.instancia.fechaHasta.toString());

                        especialidadesService.obtenerListado().then(
                            function (result) {
                                $scope.especialidades = result;
                            });

                        $rootScope.loadingLoad = false;
                    });
            }
        }
        $scope.init();

        $scope.especalidadChange = function (idEspecialidad) {
            $scope.resultadosIdEspecialidad = idEspecialidad;
        };

    }]);
