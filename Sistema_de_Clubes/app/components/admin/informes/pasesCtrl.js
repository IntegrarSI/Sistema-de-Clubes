angular.module('admin')
    .controller('informes_pasesCtrl', ['$scope', '$rootScope', 'informesService', 'excel', '$timeout', 'jugadoresService', 'federacionesService', 'asociacionesService', function ($scope, $rootScope, informesService, excel, $timeout, jugadoresService, federacionesService, asociacionesService) {

        $scope.filtro = {
            fechaDesde: new Date(new Date().getFullYear()-1, new Date().getMonth(), new Date().getDate()),
            fechaHasta: new Date()
        };

       
        $scope.titulo = 'Pases';

        $scope.obtenerNumero = function () {
            informesService.obtenerNroExportacion().then(
                function (result) {
                    $scope.numeroExp = result
                }
                );
        };

        //var tipos = new Array();
        //tipos[0] = "INTER-FEDERACION";
        //tipos[1]= "INTER-ASOCIACION";
        //tipos[2]= "INTER-CLUB";
        //$scope.tipoPase = tipos;

        $scope.tipoPase = [{
            id: 1,
            label: "INTER-FEDERACION",
        },
            {
            id: 2,
           label: "INTER-ASOCIACION",
            },
            {
                id: 3,
                label: "INTER-CLUB",
            }
        ];
       

        //$scope.filtro.tipoPase = "";



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
                    if ($scope.filtro.asociacion != null) {
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

     
        $rootScope.loadingLoad = true;

        $scope.buscar = function () {
            $scope.isLoading = true;
            informesService.pases($scope.filtro).then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.grilla = result;

                    $scope.isLoading = false;
                    $rootScope.loadingLoad = false;
                }
            );
        };

        $scope.submit = function () {
            $scope.isLoading = true;
            $scope.buscar();
            return false;
        };

        $scope.submit();

        $scope.guardarExportacion = function () {
            informesService.guardar().then(
                function (result) {
                    $scope.resultadoGuadrar = result
                }
                );
        };

        $scope.exportToExcel = function (divId) {
            $scope.guardarExportacion();
            $scope.obtenerNumero();
            var exportHref = excel.tableToExcel('#' + divId, 'WireWorkbenchDataExport');
            $timeout(function () {
                //location.href = exportHref;
                var link = document.createElement('a');
                link.download = $scope.titulo + '_' + ($scope.numeroExp + 1) + ".xls";
                link.href = exportHref;
                link.click();
            }, 500); // trigger download
        };

        $scope.print = function (divId) {
            var printContents = document.getElementById(divId).innerHTML;
            var popupWin = window.open('', '_blank');
            popupWin.document.open();
            popupWin.document.write('<html><head></head><body onload="window.print(); window.close();"><div style="width:19cm">' + printContents + '</div></body></html>');
            popupWin.document.close();
        }

        $scope.obtenerNumero()
    }]);
