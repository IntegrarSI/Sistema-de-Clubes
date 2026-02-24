angular.module('admin')
    .controller('informes_carnetsNoEntregadosCtrl', ['$scope', '$rootScope', 'informesService', 'excel', '$timeout', 'jugadoresService', function ($scope, $rootScope, informesService, excel, $timeout, jugadoresService) {

        $scope.filtro = {
            fechaDesde: new Date(new Date().getFullYear()-1, new Date().getMonth(), new Date().getDate()),
            fechaHasta: new Date()
        };

        $scope.titulo = 'Carnets no entregados';

        $scope.obtenerNumero = function () {
            informesService.obtenerNroExportacion().then(
                function (result) {
                    $scope.numeroExp = result
                }
                );
        };
        
        $rootScope.loadingLoad = true;

        $scope.buscar = function () {
            $scope.isLoading = true;
            informesService.carnetsNoEntregados($scope.filtro).then(
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
