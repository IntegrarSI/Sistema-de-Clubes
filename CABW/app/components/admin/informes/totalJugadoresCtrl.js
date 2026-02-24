angular.module('admin')
    .controller('informes_totalJugadoresCtrl', ['$scope', '$rootScope', 'informesService', 'excel', '$timeout', 'federacionesService', 'asociacionesService', 'clubesService'
        , function ($scope, $rootScope, informesService, excel, $timeout, federacionesService, asociacionesService, clubesService) {

            $scope.filtro = {};
            $scope.titulo = 'Busqueda Jugadores';

            //lista de federaciones
            if ($scope.ticket.nivel > 2 || $scope.ticket.administraContenido)  {
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
                informesService.totalJugadores($scope.filtro).then(
                    function (result) {
                        $scope.isLoading = false;
                        $scope.grilla = result;

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