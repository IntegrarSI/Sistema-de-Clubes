angular.module('admin')
    .controller('jugadores_penalizacionesCtrl',
        ['$scope', '$rootScope', '$state', '$stateParams', 'notificationService', 'jugadoresService'
        , function ($scope, $rootScope, $state, $stateParams, notificationService, jugadoresService) {
                      
            $scope.title = 'Nueva';
            $scope.permiteModificacion = true;
            $scope.penalizacion = {};

            $rootScope.loadingLoad = true;
            if ($stateParams.id != '') {

                jugadoresService.obtener($stateParams.id).then(
                    function (result) {
                        $scope.instancia = result;

                        if ($scope.instancia.penalizacionActual != null) {
                            $scope.penalizacion = $scope.instancia.penalizacionActual;
                            $scope.penalizacion.fechaInicio = stringToDate($scope.penalizacion.fechaInicio.toString());
                            $scope.penalizacion.fechaFin = stringToDate($scope.penalizacion.fechaFin.toString());

                            $scope.title = 'Modificación de';

                            //si tiene rol asociación, el estado es solicitado y el usuario es el que la creó puede modificarla


                            //si la penalización está en estado inicial y el usuario es el creador se puede modificar
                            if ($scope.penalizacion.PenalizacionesEstados.estadoInicial != true || $scope.penalizacion.Usuarios.nombreUsuario != $scope.ticket.nombreUsuario)
                                $scope.permiteModificacion = false;
                        }

                        if ($scope.ticket.nivel == 2)
                            $scope.penalizacion.requiereAprobacionFederacion = true;
                        else
                            $scope.penalizacion.requiereAprobacionFederacion = false;

                        $rootScope.loadingLoad = false;
                    });
            }

            $scope.$watch('penalizacion.fechaInicio', function () {
                if ($scope.penalizacion.dias)
                    $scope.penalizacion.fechaFin = $scope.penalizacion.fechaInicio.addDays($scope.penalizacion.dias);
            });

            $scope.$watch('penalizacion.dias', function () {
                if ($scope.penalizacion.fechaInicio)
                    $scope.penalizacion.fechaFin = $scope.penalizacion.fechaInicio.addDays($scope.penalizacion.dias);
            });

            $scope.submit = function () {
                $scope.isLoading = true;
                $scope.isSubmitted = false;
                $scope.penalizacion.idJugador = $stateParams.id;
                jugadoresService.crearPenalizacion($scope.penalizacion).then(
                    function (result) {
                        if (result != null) {
                            $state.go('jugadores');
                            $scope.isSubmitted = true;
                        }
                        else
                            notificationService.show('red', 'Ups!', 'Ocurrió un error al intentar crear la penalización');

                        $scope.isLoading = false;
                    }
                );

                return false;
            };

    }]);

