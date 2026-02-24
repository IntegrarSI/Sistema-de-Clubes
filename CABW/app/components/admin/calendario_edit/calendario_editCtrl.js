angular.module('admin')
    .controller('calendario_editCtrl',
        ['$scope', '$rootScope', '$state', '$stateParams', 'torneosService', 'notificationService', 'especialidadesService'
        , function ($scope, $rootScope, $state, $stateParams, torneosService, notificationService, especialidadesService) {
       
            $scope.horas = new Array(24);
            $scope.puntos = [];
            for (var i = 0; i < 51; i++) {
                $scope.puntos.push(i);
            }
            $scope.minutos = new Array(0, 15, 30, 45);

            $scope.esAlta = false;
            if ($stateParams.id == '') {
                $scope.title = 'Nuevo evento';
                $scope.esAlta = true;
            }
            else
                $scope.title = 'Modificación de evento';

            $scope.init = function () {
                $scope.instancia = {};
                $scope.actividad = {};
                $scope.instancia.TorneosActividades = [];
                $scope.zona = {};
                $scope.instancia.TorneosZonas = [];
                $scope.equipo = {};
                $scope.instancia.TorneosEquipos = [];
                $scope.partido = {};
                $scope.instancia.TorneosPartidos = [];
                $scope.instancia.TorneosEspecialidades = [];

                $scope.isLoading = true;
                $rootScope.loadingLoad = true;

                if ($stateParams.id != '') {
                    //es una modificacion
                    torneosService.obtener($stateParams.id).then(
                        function (result) {
                            $scope.instancia = result;
                            $scope.instancia.fechaDesde = stringToDate($scope.instancia.fechaDesde.toString());
                            $scope.instancia.fechaHasta = stringToDate($scope.instancia.fechaHasta.toString());

                            especialidadesService.obtenerListado().then(
                                function (result) {
                                    $scope.especialidades = result;
                                    for (var i = 0; i < $scope.especialidades.length; i++) {
                                        $scope.especialidades[i].check = especialidadesChequear($scope.especialidades[i].id);
                                    }

                                    $scope.isLoading = false;
                                    $rootScope.loadingLoad = false;
                                });
                        });
                }
                else {
                    especialidadesService.obtenerListado().then(
                        function (result) {
                            $scope.especialidades = result;
                            $scope.isLoading = false;
                            $rootScope.loadingLoad = false;
                        });
                }
            }
            $scope.init();

            $scope.submit = function () {
                $scope.isLoading = true;
                torneosService.modificar($scope.instancia).then(
                    function (result) {
                        if (result.success == 'true') {
                            notificationService.show('green', 'Perfecto!', 'La modificación fue realizada correctamente');
                        }
                        else
                            notificationService.show('red', 'Ups!', result.message);

                        $scope.isLoading = false;
                    }
                );

                return false;
            };

            /* Especialidades */
            $scope.toggleEspecialidad = function (item) {
                var torneoEspecialidad = { idTorneo: $scope.instancia.id, idEspecialidad: item.id, Especialidades: item };

                //se verifica si ya existe la especialidad
                var existeEspecialidad = -1;
                for (var i = 0; i < $scope.instancia.TorneosEspecialidades.length && existeEspecialidad == -1; i++) {
                    if ($scope.instancia.TorneosEspecialidades[i].idEspecialidad == item.id)
                        existeEspecialidad = i;
                }

                if (existeEspecialidad == -1) {
                    $scope.instancia.TorneosEspecialidades.push(torneoEspecialidad);
                } else {
                    $scope.instancia.TorneosEspecialidades.splice(existeEspecialidad, 1);
                }

                $scope.instancia.especialidad = '';
                for (var i = 0; i < $scope.instancia.TorneosEspecialidades.length; i++) {
                    $scope.instancia.especialidad += ($scope.instancia.especialidad.length > 0 ? ', ' : '') + $scope.instancia.TorneosEspecialidades[i].Especialidades.descripcion;
                }
            };

            function especialidadesChequear(idEspecialidad) {
                for (var i = 0; i < $scope.instancia.TorneosEspecialidades.length; i++) {
                    if ($scope.instancia.TorneosEspecialidades[i].idEspecialidad == idEspecialidad)
                        return true;
                }
                return false;
            }

            $scope.especalidadChange = function (idEspecialidad) {
                $scope.resultadosIdEspecialidad = idEspecialidad;
            };

            $scope.agregarActividad = function () {
                $scope.instancia.TorneosActividades.push(angular.copy($scope.actividad));
                $scope.actividad = {};
            };

            $scope.eliminarActividad = function (item) {
                $scope.instancia.TorneosActividades.splice($scope.instancia.TorneosActividades.indexOf(item), 1);
            };

            $scope.agregarZona = function () {
                $scope.zona.idEspecialidad = $scope.resultadosIdEspecialidad;
                $scope.instancia.TorneosZonas.push(angular.copy($scope.zona));
                $scope.zona = {};
            };2

            $scope.eliminarZona = function (item) {
                $scope.instancia.TorneosZonas.splice($scope.instancia.TorneosZonas.indexOf(item), 1);
            };

            $scope.agregarEquipo = function () {
                $scope.equipo.idEspecialidad = $scope.resultadosIdEspecialidad;
                $scope.instancia.TorneosEquipos.push(angular.copy($scope.equipo));
                $scope.equipo = {};
            };

            $scope.eliminarEquipo = function (item) {
                $scope.instancia.TorneosEquipos.splice($scope.instancia.TorneosEquipos.indexOf(item), 1);
            };

            /* Partidos */

            $scope.agregarPartido = function () {
                $scope.partido.idEspecialidad = $scope.resultadosIdEspecialidad;
                $scope.instancia.TorneosPartidos.push(angular.copy($scope.partido));
                $scope.partido = {};
            };

            $scope.eliminarPartido = function (item) {
                $scope.instancia.TorneosPartidos.splice($scope.instancia.TorneosPartidos.indexOf(item), 1);
            };

            $scope.puntosEquipo1Change = function () {
                if ($scope.partido.puntosEquipo1 != null && $scope.partido.puntosEquipo1 != '15')
                    $scope.partido.puntosEquipo2 = '15';
            }

            $scope.puntosEquipo2Change = function () {
                if ($scope.partido.puntosEquipo2 != null && $scope.partido.puntosEquipo2 != '15')
                    $scope.partido.puntosEquipo1 = '15';
            }


    }]);
