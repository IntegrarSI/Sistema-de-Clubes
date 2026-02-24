angular.module('site')
    .controller('calendarioCtrl', ['$scope', '$rootScope', 'db', 'torneosService', function ($scope, $rootScope, db, torneosService) {
        
        //se busacan los eventos activos
        $scope.isLoading = true;

        $scope.init = function () {
            $scope.isLoading = true;
            $rootScope.loadingLoad = true;
            $scope.meses = [];
            torneosService.obtenerListado($scope.filter, $scope.currentPage).then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.torneos = result;
                    $scope.obtenerMeses();

                    $rootScope.loadingLoad = false;
                }
            );
        };

        $scope.obtenerMeses = function () {
            var ultimoMes = 0;
            var ultimoAno = 0;
            var cantidadEventosMes = 0;
            var fecha;
            var offsetBootstrap;
            var nroEvento = 0;
            for (var i = 0; i < $scope.torneos.length; i++) {
                fecha = stringToDate($scope.torneos[i].fechaDesde);
                fechaHasta = stringToDate($scope.torneos[i].fechaHasta);

                if (fecha.getMonth() + 1 != ultimoMes || fecha.getYear() + 1900 != ultimoAno) {

                    if (ultimoAno != 0) {
                        if (cantidadEventosMes <= 3)
                            offsetBootstrap = (12 - cantidadEventosMes * 4) / 2;
                        $scope.meses.push({ mes: ultimoMes, ano: ultimoAno, nombreMes: nombreMes(ultimoMes), cantidadEventos: cantidadEventosMes, offset: offsetBootstrap });
                    }

                    ultimoMes = fecha.getMonth() + 1;
                    ultimoAno = fecha.getYear() + 1900;

                    cantidadEventosMes = 0;
                    nroEvento = 0;
                }

                nroEvento++;
                $scope.torneos[i].mes = ultimoMes;
                $scope.torneos[i].ano = ultimoAno;
                $scope.torneos[i].diaDesde = fecha.getDate();
                $scope.torneos[i].mesDesde = nombreMes(fecha.getMonth() + 1);
                $scope.torneos[i].diaHasta = fechaHasta.getDate();
                $scope.torneos[i].mesHasta = nombreMes(fechaHasta.getMonth() + 1);
                $scope.torneos[i].nroEvento = nroEvento;

                cantidadEventosMes++;
            }

            if (ultimoAno != 0) {
                if (cantidadEventosMes <= 3)
                    offsetBootstrap = (12 - cantidadEventosMes * 4) / 2;
                else if (cantidadEventosMes == 4)
                    offsetBootstrap = 3;
                $scope.meses.push({ mes: ultimoMes, ano: ultimoAno, nombreMes: nombreMes(ultimoMes), cantidadEventos: cantidadEventosMes, offset: offsetBootstrap });
            }
        }

        $scope.init();

    }]);
