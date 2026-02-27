angular.module('admin')
    .controller('jugadoresCtrl', ['$scope', '$rootScope', '$window', 'jugadoresService', 'federacionesService', 'asociacionesService', 'clubesService', 'auth', 'notificationService', 'confirmarEliminarUsuario', function ($scope, $rootScope, $window, jugadoresService, federacionesService, asociacionesService, clubesService, auth, notificationService, confirmarEliminarUsuario) {


        $scope.ticket = auth.me();

        $scope.logout = function () {
            auth.logout();
        }




        $scope.isLoading = true;
        $scope.filtro = {};
        $(".filtroAvanzado").css('display', 'none');


        
        $scope.controlarCheckbox = function () {
            if ($('#chkBuscarTodos').prop('checked')) {
                $(".filtroAvanzado").fadeIn('slow');
                $scope.filtro.checkbox = true;
            }
            else {
                $(".filtroAvanzado").fadeOut('slow');
                $scope.filtroAsociacion = '';
                $scope.filtroFederacion = '';
                $scope.filtrofiltro = '';
                $scope.filtroClub = '';
                $scope.filtro.checkbox = false;
            }
        };



        //lista de federaciones
        if ($scope.ticket.nivel > 2) {
            federacionesService.obtenerLista().then(
                    function (result) {
                        $scope.federaciones = result;
                        if ($scope.federaciones != null) {
                            $scope.filtro.federacion = $scope.federaciones[0];
                            
                        }

                        
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

        

        $scope.filtroAsociacion = '';
        $scope.filtroFederacion = '';
        $scope.filtrofiltro = '';
        $scope.filtroClub = '';


        $scope.submit = function () {
            $scope.buscar();
            if ($('#chkBuscarTodos').prop('checked')) {
                $window.localStorage.jugadores = JSON.stringify($scope.filtro);
            }
            else {
                $scope.filtro.asociacion = null;
                $scope.filtro.federacion = null;
                $scope.filtro.club = null;
                $window.localStorage.jugadores = JSON.stringify($scope.filtro);
            }

            return false;
        };


        $rootScope.loadingLoad = true;
        $scope.buscar = function () {
            //Se activa la busqueda avanzada si se selecciono "Busqueda Avanzada" 
            if ($scope.filtro.checkbox == true) {
                    if ($scope.filtro.asociacion != null) {
                        $scope.filtroAsociacion = $scope.filtro.asociacion.descripcion;
                    }
               
                    if ($scope.filtro.federacion != null) {
                        $scope.filtroFederacion = $scope.filtro.federacion.descripcion;
                    }
                    if ($scope.filtro.club != null) {
                        $scope.filtroClub = $scope.filtro.club.descripcion;
                    }
  
            }
 
            //Busca siempre por este campo, filtro comun
            if ($scope.filtro.buscar != null) {
                $scope.filtrofiltro = $scope.filtro.buscar;
            }

            $scope.isLoading = true;
            jugadoresService.obtenerListado($scope.filtrofiltro, $scope.filtroFederacion, $scope.filtroAsociacion, $scope.filtroClub, $scope.currentPage).then(
                    function (result) {
                        $scope.isLoading = false;
                        $scope.grilla = result.jugadores.paginaActual;

                        today = new Date();
                        mes = today.getMonth() + 1
                        dia = today.getDate()
                        ano = today.getFullYear()
                        if (mes < 10) {
                            mes = '0' + mes
                        }

                        if (dia < 10) {
                            dia = '0' + dia
                        }
                        fecha = dd = ano + "-" + mes + "-" + dia

                        for (var i = 0; i < $scope.grilla.length; i++) {
                            if ($scope.grilla[i].JugadoresHabilitaciones.length > 0) {
                                for (var j = 0; j < $scope.grilla[i].JugadoresHabilitaciones.length; j++) {
                                    if ($scope.grilla[i].JugadoresHabilitaciones[j].fechaInhabilitacion > fecha) {

                                        $scope.grilla[i].habilitado = true;
                                        break;
                                    }
                                    else {
                                        $scope.grilla[i].habilitado = false;
                                        
                                    }
                                }
                            }
                            else {
                                $scope.grilla[i].habilitado = false;
                            }
                            if ($scope.grilla[i].JugadoresSolicitudesCarnets.length > 0) {
                                var fechaMayor = 0;
                                var mayor = 0;
                                for (var j = 0; j < $scope.grilla[i].JugadoresSolicitudesCarnets.length; j++) {
                                    if (fechaMayor == 0) {
                                        fechaMayor = $scope.grilla[i].JugadoresSolicitudesCarnets[j].fechaSolicitud;
                                        mayor = j;
                                    }
                                    else {
                                        if (fechaMayor < $scope.grilla[i].JugadoresSolicitudesCarnets[j].fechaSolicitud) {
                                            fechaMayor = $scope.grilla[i].JugadoresSolicitudesCarnets[j].fechaSolicitud;
                                            mayor = j;
                                        }
                                    }
                                }

                                if ($scope.grilla[i].JugadoresSolicitudesCarnets[mayor].fechaEntrega == null) {

                                    $scope.grilla[i].solicitaCarnet = true;
                                }
                                else {
                                    $scope.grilla[i].solicitaCarnet = false;
                                }
                            }
                            else
                            {
                                $scope.grilla[i].solicitaCarnet = false;
                            }
                        }

                        $scope.totalItems = result.jugadores.totalItems;
                        $rootScope.loadingLoad = false;

                        $scope.filtroAsociacion = '';
                        $scope.filtroFederacion = '';
                        $scope.filtrofiltro = '';
                        $scope.filtroClub = '';
                    }
         
                );

        };


        $scope.loadFilter = function () {

            if ($window.localStorage.jugadores != null && $window.localStorage.jugadores != "undefined") {
                $scope.filtro = JSON.parse($window.localStorage.jugadores);
                if ($scope.filtro.checkbox == true) {
                    $(".filtroAvanzado").fadeIn('slow');
                    $("chkBuscarTodos").prop("checked", true);
                }
                else {
                    $(".filtroAvanzado").fadeOut('slow');
                    $("chkBuscarTodos").prop("checked", false);
                }
            }
            else {
                $scope.filtro = {};
            }
        }

        $scope.eliminarJugador = function (id) {




            confirmarEliminarUsuario.showModal()
                .then(function (result) {
                    if (result == 'aceptar') {
                        jugadoresService.eliminar(id).then(
                            function (result) {
                                if (result.success == 'true') {
                                    notificationService.show('green', 'Perfecto!', 'El usuario se elimino correctamente');

                                }
                                else {
                                    notificationService.show('red', 'Ups!', result.message);
                                }
                                $scope.isLoading = false;
                                $scope.buscar();

                          });
                    }
                });
           
        }
          
        
        //paginacion
        $scope.viewby = 10;
        $scope.totalItems = 0;
        $scope.currentPage = 1;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5; //Number of pager buttons to show
        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };
        $scope.pageChanged = function () {
            $scope.buscar();
        };
        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1; //reset to first paghe
        }

        $scope.loadFilter();
        $scope.buscar();
   
    }]);
