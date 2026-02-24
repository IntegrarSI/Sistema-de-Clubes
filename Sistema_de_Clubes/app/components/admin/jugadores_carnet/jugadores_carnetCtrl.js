angular.module('admin')
    .controller('jugadores_carnetCtrl',
        ['$scope', '$rootScope', '$state', '$stateParams', 'jugadoresService'
        , function ($scope, $rootScope, $state, $stateParams, jugadoresService) {
       
            $scope.fechaImpresion = new Date();
                
            if ($stateParams.id != '') {
                $rootScope.loadingLoad = true;
                jugadoresService.obtener($stateParams.id).then(
                    function (result) {
                        $scope.instancia = result;
                        //Esto es provisorio hasta el 1 septiembre de 2017--APIE
                        $scope.fechaImpresion = Date.now();
                        //se calcula la fecha de impresion 
                        //if ($scope.instancia.estampillaActual != null) {
                        //    $scope.fechaImpresion = $scope.instancia.estampillaActual.fechaHabilitacion;
                        //}
                        //else {
                        //    $scope.fechaImpresion = $scope.instancia.fechaIngreso;
                        //}
                        //if ($scope.instancia.JugadoresPases.length > 0 && $scope.instancia.JugadoresCategorias.length > 0) {
                        //    if ($scope.instancia.JugadoresPases[0].fechaPase > 0 && $scope.instancia.JugadoresCategorias[0].fecha) {
                        //        if ($scope.fechaImpresion < $scope.instancia.JugadoresPases[0].fechaPase) {
                        //            $scope.fechaImpresion = $scope.instancia.JugadoresPases[0].fechaPase;
                        //        }
                        //    }
                        //    else {
                        //        if ($scope.fechaImpresion < $scope.instancia.JugadoresCategorias[0].fecha) {
                        //            $scope.fechaImpresion = $scope.instancia.JugadoresCategorias[0].fecha;
                        //        }
                        //    }
                        //}
                        //else {

                        //    if ($scope.instancia.JugadoresPases.length > 0) {
                        //        if ($scope.fechaImpresion < $scope.instancia.JugadoresPases[0].fechaPase) {
                        //            $scope.fechaImpresion = $scope.instancia.JugadoresPases[0].fechaPase;
                        //        }
                        //    }

                        //    if ($scope.instancia.JugadoresCategorias.length > 0) {
                        //        if ($scope.fechaImpresion < $scope.instancia.JugadoresCategorias[0].fecha) {
                        //            $scope.fechaImpresion = $scope.instancia.JugadoresCategorias[0].fecha;
                        //        }

                        //    }

                        //}
                        
                        $scope.instancia.fechaNacimiento = stringToDate($scope.instancia.fechaNacimiento.toString());
                        if ($scope.instancia.Localidades != null)
                            $scope.instancia.localidad = $scope.instancia.Localidades.descripcion + ' - ' + $scope.instancia.Localidades.Provincias.descripcion;
                           

                        $rootScope.loadingLoad = false;
                    });
            }

    }]);

