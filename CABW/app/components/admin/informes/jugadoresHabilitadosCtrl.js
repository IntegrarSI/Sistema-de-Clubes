angular.module('admin')
    .controller('informes_jugadoresHabilitadosCtrl', ['$scope', '$rootScope', 'informesService', 'excel', '$timeout', 'federacionesService', 'asociacionesService', 'clubesService'
        , function ($scope, $rootScope, informesService, excel, $timeout, federacionesService, asociacionesService, clubesService) {

        $scope.filtro = {};
        $scope.titulo = 'Jugadores habilitados';
        

        //lista de federaciones
        if ($scope.ticket.nivel > 2 || $scope.ticket.administraContenido) {
            federacionesService.obtenerLista().then(
                    function (result) {
                        $scope.federaciones = result;
                        if ($scope.federaciones != null)
                            $scope.filtro.federacion = $scope.federaciones[0];
                    }
                );
        }
        else {
            //lista de asociaciones
            asociacionesService.obtenerListaPorUsuario().then(
                function (result) {
                    $scope.asociaciones = result;
                    if ($scope.asociaciones != null) {
                        $scope.federaciones = [$scope.asociaciones[0].Federaciones];
                        $scope.filtro.federacion = $scope.asociaciones[0].Federaciones;
                    }
                }
            );
        }

        $scope.$watch('filtro.federacion', function () {
            //se cargan las asociaciones cuándo cambia la federacion
            if ($scope.filtro.federacion) {
                asociacionesService.obtenerLista($scope.filtro.federacion.id).then(
                    function (result) {
                        $scope.asociaciones = result;
                    }
                );
            }
        });

        $scope.$watch('filtro.asociacion', function () {
            //se cargan las asociaciones cuándo cambia la federacion
            if ($scope.filtro.asociacion) {
                clubesService.obtenerLista($scope.filtro.asociacion.id).then(
                    function (result) {
                        $scope.clubes = result;
                    }
                );
            }
        });

        $rootScope.loadingLoad = true;

        $scope.buscar = function () {
            $scope.isLoading = true;
            informesService.jugadoresHabilitados($scope.filtro).then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.grilla = result;
                    //Contar la cantidad de Jugadores Habilitados (se hace esto porque puede haber 1 jugador con 2 estampillas en la lista)
                    var listaAuxJugadores = [] 
                    var listaVeteranos = [] //tipo A
                    var listaInfantiles = [] //tipo I
                 
                    for (var i = 0; i < $scope.grilla.length; i++) {
                        if (!($scope.grilla[i].DNI in listaAuxJugadores)) {
                            listaAuxJugadores.push($scope.grilla[i].DNI)
                        }
                        if (!($scope.grilla[i].DNI in listaVeteranos)) {
                            if ($scope.grilla[i].tipo == 'A') { listaVeteranos.push($scope.grilla[i].DNI) }
                        }
                        if (!($scope.grilla[i].DNI in listaInfantiles) && $scope.grilla[i].tipo == 'I') {
                            listaInfantiles.push($scope.grilla[i].DNI)
                           }
                    }
                    jQuery.unique(listaAuxJugadores)
                    $scope.TotalGrilla = listaAuxJugadores.length;
                    jQuery.unique(listaVeteranos)
                    $scope.TotalVeteranos = listaVeteranos.length;
                    jQuery.unique(listaInfantiles)
                    $scope.TotalInfantiles = listaInfantiles.length;
                    $scope.TotalMayores = listaAuxJugadores.length - listaVeteranos.length - listaInfantiles.length

                    $scope.isLoading = false;
                    $rootScope.loadingLoad = false;
                }
            );
        };

        $scope.submit = function () {
            $scope.buscar();
            return false;
        };

        $scope.submit();
        
        $scope.exportToExcel = function (divId) {
            //$scope.jugadoresHabilitadosExport($scope.filtro).then(
            //    function (result) {
            //        $scope.isLoading = false;
            //        $scope.exp = result;
            //        $scope.isLoading = false;
            //        $rootScope.loadingLoad = false;
            //    }

            //);
            var exportHref = excel.tableToExcel('#' + divId, 'WireWorkbenchDataExport');
            $timeout(function () {
                //location.href = exportHref;
                var link = document.createElement('a');
                link.download = $scope.titulo + ".xls";
                link.href = exportHref;
                link.click();
            }, 100); // trigger download
        };

        $scope.print = function (divId) {
            var printContents = document.getElementById(divId).innerHTML;
            var popupWin = window.open('', '_blank');
            popupWin.document.open();
            popupWin.document.write('<html><head></head><body onload="window.print(); window.close();"><div style="width:19cm">' + printContents + '</div></body></html>');
            popupWin.document.close();
        }

    }]);
