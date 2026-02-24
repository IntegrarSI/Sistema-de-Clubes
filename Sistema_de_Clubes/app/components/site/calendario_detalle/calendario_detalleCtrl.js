angular.module('site')
    .controller('calendario_detalleCtrl', ['$scope', '$rootScope', '$stateParams', 'torneosService', function ($scope, $rootScope, $stateParams, torneosService) {

        $scope.diasActividades = [];

        $scope.year = Date.now();

        $scope.init = function () {
            $scope.isLoading = true;
            if ($stateParams.id != '') {
                $rootScope.loadingLoad = true;
                torneosService.obtener($stateParams.id).then(
                    function (result) {
                        $scope.isLoading = false;
                        $scope.instancia = result;
                        if ($scope.instancia.resultadosDescripcion != null)
                            $scope.instancia.detalleDescripcion = $scope.instancia.resultadosDescripcion;

                        if ($scope.instancia.detalleDescripcion != null)
                            $scope.instancia.detalleDescripcion = $scope.instancia.detalleDescripcion.replace('\n', '<br/><br/>');

                        /*$scope.instancia.fechaDesde = stringToDate($scope.instancia.fechaDesde.toString());
                        $scope.instancia.fechaHasta = stringToDate($scope.instancia.fechaHasta.toString());*/
                        $scope.obtenerDiasActividades();

                        $rootScope.loadingLoad = false;
                    });
            }
        }

        $scope.obtenerDiasActividades = function () {
            if ($scope.instancia != null && $scope.instancia.TorneosActividades.length > 0) {
                var ultimoDia = 0;
                var ultimoMes = 0;
                var ultimaFecha = null;
                var cantidadEventosDia = 0;
                var fecha;
                var nroEvento = 0;
                for (var i = 0; i < $scope.instancia.TorneosActividades.length; i++) {
                    fecha = stringToDate($scope.instancia.TorneosActividades[i].fecha);

                    if (fecha.getMonth() + 1 != ultimoMes || fecha.getDate() != ultimoDia) {

                        if (ultimoDia != 0)
                            $scope.diasActividades.push({ dia: ultimoDia, nombreDia: nombreDia(ultimaFecha.getDay()), mes: ultimoMes, nombreMes: nombreMes(ultimoMes), cantidadEventos: cantidadEventosDia, fecha: angular.copy(fecha) });

                        ultimoMes = fecha.getMonth() + 1;
                        ultimoDia = fecha.getDate();
                        ultimaFecha = fecha;

                        cantidadEventosDia = 0;
                        nroEvento = 0;
                    }

                    nroEvento++;
                    cantidadEventosDia++;

                    $scope.instancia.TorneosActividades[i].dia = ultimoDia;
                }

                if (ultimoDia != 0) {
                    $scope.diasActividades.push({ dia: ultimoDia, nombreDia: nombreDia(ultimaFecha.getDay()), mes: ultimoMes, nombreMes: nombreMes(ultimoMes), cantidadEventos: cantidadEventosDia, fecha: angular.copy(fecha) });
                }
            }
        }

        $scope.init();

    }]);
