angular.module('admin')
    .controller('jugadores_pasesCtrl',
        ['$scope', '$rootScope', '$state', '$stateParams', 'notificationService', 'jugadoresService', 'federacionesService', 'asociacionesService', 'clubesService'
        , function ($scope, $rootScope, $state, $stateParams, notificationService, jugadoresService, federacionesService, asociacionesService, clubesService) {
                      
            federacionesService.obtenerListaCompleta().then(
                    function (result) {
                        $scope.federaciones = result;
                    }
                );

            $scope.pase = {};

            $rootScope.loadingLoad = true;
            if ($stateParams.id != '') {

                jugadoresService.obtener($stateParams.id).then(
                    function (result) {
                        $scope.instancia = result;
                        $scope.instancia.fechaNacimiento = stringToDate($scope.instancia.fechaNacimiento.toString());
                        if ($scope.instancia.Localidades != null)
                            $scope.instancia.localidad = $scope.instancia.Localidades.descripcion + ' - ' + $scope.instancia.Localidades.Provincias.descripcion;

                        if ($scope.instancia.diasUltimoPase != null && $scope.instancia.diasUltimoPase <90)
                            $scope.paseExistente = true;
                        else
                            $scope.paseExistente = false;

                        $rootScope.loadingLoad = false;
                        $scope.pase.idCategoria=$scope.instancia.idCategoria
                    });
            }

            $scope.$watch('pase.idFederacionDestino', function () {
                //se cargan las asociaciones cuándo cambia la federacion
                if ($scope.pase.idFederacionDestino) {
                    asociacionesService.obtenerLista($scope.pase.idFederacionDestino).then(
                        function (result) {
                            $scope.asociaciones = result;
                        }
                    );
                }

                if ($scope.instancia && $scope.pase.idFederacionDestino != $scope.instancia.Clubes.Asociaciones.idFederacion)
                    $scope.pase.tipoPase = "INTER-FEDERACION";
                else
                    $scope.pase.tipoPase = "";
            });

            $scope.$watch('pase.idAsociacionDestino', function () {
                //se cargan las asociaciones cuándo cambia la federacion
                if ($scope.pase.idAsociacionDestino) {
                    clubesService.obtenerLista($scope.pase.idAsociacionDestino).then(
                        function (result) {
                            $scope.clubes = result;
                        }
                    );
                }

                if ($scope.instancia && $scope.pase.idFederacionDestino != $scope.instancia.Clubes.Asociaciones.idFederacion)
                    $scope.pase.tipoPase = "INTER-FEDERACION";
                else if ($scope.instancia && $scope.pase.idAsociacionDestino != $scope.instancia.Clubes.idAsociacion)
                    $scope.pase.tipoPase = "INTER-ASOCIACION";
                else if ($scope.pase.idAsociacionDestino)
                    $scope.pase.tipoPase = "INTER-CLUB";
            });

            $scope.submit = function () {
                $scope.isLoading = true;
                $scope.isSubmitted = false;
                $scope.pase.idJugador = $stateParams.id;
                jugadoresService.crearPase($scope.pase).then(
                    function (result) {
                        if (result != null) {
                            $scope.pase = result;
                            $scope.isSubmitted = true;
                        }
                        else
                            notificationService.show('red', 'Ups!', 'Ocurrió un error al intentar crear el pase');

                        $scope.isLoading = false;
                    }
                );

                return false;
            };

    }]);

