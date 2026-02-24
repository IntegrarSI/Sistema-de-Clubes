angular.module('admin')
    .controller('jugadores_editCtrl',
        ['$scope', '$rootScope', '$state', '$filter', 'focus', '$stateParams', 'notificationService', 'jugadoresService', 'federacionesService', 'asociacionesService', 'clubesService', 'localidadesService', 'FileUploader', 'categoriasService', 'modal_habilitacion', 'confirmarEliminar', 'auth'
            , function ($scope, $rootScope, $state, $filter, focus, $stateParams, notificationService, jugadoresService, federacionesService, asociacionesService, clubesService, localidadesService, FileUploader, categoriasService, modal_habilitacion, confirmarEliminar, auth) {



                $scope.ticket = auth.me();

                $scope.logout = function () {
                    auth.logout();
                }



                $scope.esAlta = false;
                if ($stateParams.id == '') {
                    $scope.title = 'Nuevo Jugador';
                    $scope.esAlta = true;
                }
                else
                    $scope.title = 'Modificación de Jugadores';


                /* Uploader */
                var uploader = $scope.uploader = new FileUploader({
                    url: '/jugadores/UploadFile',
                    autoUpload: true
                });

                uploader.isHTML5 = true;

                // FILTERS

                uploader.filters.push({
                    name: 'imageFilter',
                    fn: function (item, options) {
                        var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                        return '|jpg|png|jpeg|'.indexOf(type) !== -1;
                    }
                });

                uploader.onProgressItem = function (fileItem, progress) {
                    $scope.isLoading = true;
                };
                uploader.onSuccessItem = function (fileItem, response, status, headers) {
                    if (response.success)
                        $scope.instancia.foto = response.fileName;

                    $scope.isLoading = false;
                };
                uploader.onErrorItem = function (fileItem, response, status, headers) {
                    $scope.isLoading = false;
                };


                if ($scope.ticket.roles.includes('confederacion'))
                    $scope.items = [{ enable: true }]

                else
                    $scope.items = [{ disabled: true }]



                $scope.fechaHabilitacionDesde = new Date();
                $scope.fechaHabilitacionHasta = new Date(new Date().getYear() + 1901, 2, 31); //1901 para que sume 1 año mas

                $scope.init = function () {

                    $scope.botonSolicitarCarnet = false;
                    $scope.botonEntregarCarnet = false;

                    categoriasService.obtenerLista().then(
                        function (result) {
                            $scope.categorias = result;
                        }
                    );

                    //lista de federaciones
                    if ($scope.ticket.nivel > 2) {
                        federacionesService.obtenerLista().then(
                            function (result) {
                                $scope.federaciones = result;
                            }
                        );
                    }
                    else {
                        //lista de asociaciones
                        asociacionesService.obtenerListaPorUsuario().then(
                            function (result) {
                                $scope.asociaciones = result;
                                if ($scope.asociaciones != null)
                                    $scope.federaciones = [$scope.asociaciones[0].Federaciones];
                            }
                        );
                    }


                    $scope.instancia = {};
                    $scope.localidad = {};
                    $scope.habilitacion = {};
                    $scope.categoria = {};
                    $scope.Puntaje = {};
                    $scope.nroCarnet = {};


                    $scope.isLoading = false;
                    focus('apellido');

                    if ($stateParams.id != '') {

                        //es una modificacion
                        $scope.esModificacion = true;
                        $scope.isLoading = true;
                        $rootScope.loadingLoad = true;
                        jugadoresService.obtener($stateParams.id).then(
                            function (result) {
                                $scope.instancia = result;
                                $scope.instancia.fechaNacimiento = stringToDate($scope.instancia.fechaNacimiento.toString());
                                if ($scope.instancia.fechaIngreso != null)
                                    $scope.instancia.fechaIngreso = stringToDate($scope.instancia.fechaIngreso.toString());
                                $scope.instancia.idFederacion = $scope.instancia.Clubes.Asociaciones.idFederacion;
                                $scope.instancia.idAsociacion = $scope.instancia.Clubes.idAsociacion;

                                if ($scope.instancia.Localidades != null)
                                    $scope.instancia.localidad = $scope.instancia.Localidades.descripcion + ' - ' + $scope.instancia.Localidades.Provincias.descripcion;

                                //se verifica si puede solicitar carnet
                                if ($scope.instancia.JugadoresSolicitudesCarnets.length > 0 && $scope.instancia.JugadoresSolicitudesCarnets[$scope.instancia.JugadoresSolicitudesCarnets.length - 1].fechaEntrega == null) {
                                    $scope.botonSolicitarCarnet = false;
                                    $scope.botonEntregarCarnet = true;
                                } else {
                                    $scope.botonSolicitarCarnet = true;
                                    $scope.botonEntregarCarnet = false;
                                }

                                $scope.federaciones = [$scope.instancia.Clubes.Asociaciones.Federaciones];
                                $scope.asociaciones = [$scope.instancia.Clubes.Asociaciones];
                                $scope.clubes = [$scope.instancia.Clubes];

                                //se deshabilita la categoria dependiendo de los permisos del usuario
                                //la mejor categoría es PRIMERA.
                                //es posible mejorarle la categoría pero sólo confederación puede bajarle la categoría
                                if ($scope.ticket.nivel < 4) {
                                    //se eliminan las categorías inferiores a la categoría actual del jugador
                                    for (var i = 0; i < $scope.categorias.length; i++) {
                                        if ($scope.categorias[i].nivel <= $scope.instancia.Categorias.nivel)
                                            $scope.categorias[i].disabled = true;
                                    }

                                }


                                $scope.isLoading = false;
                                $rootScope.loadingLoad = false;
                            });
                    }
                    else {

                        $scope.instancia.activo = true;
                        //precargo federacion y categoria
                        $scope.instancia.idFederacion = 25;
                        $scope.instancia.idCategoria = 1;

                    }
                }

                //Se comento porque cuando se eligue una provincia no tiene en cuenta el usuario

                $scope.$watch('instancia.idFederacion', function () {
                    //se cargan las asociaciones cuándo cambia la federacion
                    if ($stateParams.id == '') {
                        if ($scope.instancia.idFederacion) {
                            asociacionesService.obtenerLista($scope.instancia.idFederacion).then(
                                function (result) {
                                    $scope.asociaciones = result;
                                }
                            );
                        }
                    }
                });

                $scope.$watch('instancia.idAsociacion', function () {
                    //se cargan las asociaciones cuándo cambia la federacion
                    if ($stateParams.id == '') {
                        if ($scope.instancia.idAsociacion) {
                            clubesService.obtenerLista($scope.instancia.idAsociacion).then(
                                function (result) {
                                    $scope.clubes = result;
                                }
                            );
                        }
                    }
                });

                $scope.init();



                $scope.localidades = function (filter) {
                    $scope.localidad = null;
                    return localidadesService.obtenerListaAutocompletar(filter).then(
                        function (data) {
                            return data;
                        }
                    );
                };

                $scope.localidadesSelect = function ($item) {
                    $scope.instancia.localidad = $item.descripcion + " - " + $item.Provincias.descripcion;
                    $scope.instancia.idLocalidad = $item.id;
                    $scope.instancia.Localidades = $item;
                };

                $scope.entregarCarnet = function () {
                    $scope.isLoading = true;
                    $scope.botonEntregarCarnet = false;

                    var solicitudCarnet = $scope.instancia.JugadoresSolicitudesCarnets[$scope.instancia.JugadoresSolicitudesCarnets.length - 1];
                    jugadoresService.entregarCarnet(solicitudCarnet).then(
                        function (result) {
                            if (result != null) {
                                notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                                $scope.instancia.JugadoresSolicitudesCarnets[$scope.instancia.JugadoresSolicitudesCarnets.length - 1] = result;
                            }
                            else {
                                notificationService.show('red', 'Ups!', 'Ocurrió un error. Por favor vuelva a intentar en unos minutos');
                                $scope.botonEntregarCarnet = true;
                            }

                            $scope.isLoading = false;
                        }
                    );
                };



                $scope.solicitarCarnet = function (motivo) {
                    $scope.isLoading = true;
                    $scope.botonSolicitarCarnet = false;

                    jugadoresService.solicitarCarnet($stateParams.id, motivo).then(
                        function (result) {
                            if (result != null) {
                                notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                                $scope.instancia.JugadoresSolicitudesCarnets.push(result);

                                $scope.botonSolicitarCarnet = false;
                                $scope.botonEntregarCarnet = true;
                            }
                            else {
                                notificationService.show('red', 'Ups!', 'Ocurrió un error. Por favor vuelva a intentar en unos minutos');
                                $scope.botonSolicitarCarnet = true;
                            }

                            $scope.isLoading = false;
                        }
                    );
                };

                $scope.guardar = function () {
                    $scope.isLoading = true;
                    //$scope.instancia.logo = uploader.queue[0].file;
                    jugadoresService.modificar($scope.instancia).then(
                        function (result) {
                            if (result.success == 'true') {
                                notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                                $state.go('jugadores');
                            }
                            else
                                notificationService.show('red', 'Ups!', result.message);

                            $scope.isLoading = false;
                        }
                    );

                    return false;
                };
                //VERIFICAR HABILITACION
                $scope.submitHabilitacion = function () {
                    $scope.isLoadingHabilitacion = true;

                    $scope.habilitacion.idJugador = $stateParams.id;
                    $scope.habilitacion.fechaHabilitacion = $scope.fechaHabilitacionDesde;
                    $scope.habilitacion.fechaInhabilitacion = $scope.fechaHabilitacionHasta;

                    jugadoresService.obtenerEdad($stateParams.id).then(
                        function (result) {
                            $scope.habilitacion.categoria = result;
                            if (($scope.habilitacion.categoria == "M" && ($scope.habilitacion.numero >= 00001 && $scope.habilitacion.numero <= 16000)) ||
                                ($scope.habilitacion.categoria == "I" && ($scope.habilitacion.numero >= 20001 && $scope.habilitacion.numero <= 21500)) ||
                                ($scope.habilitacion.categoria == "A" && $scope.habilitacion.numero >= 25001 && $scope.habilitacion.numero <= 25500)) {
                                jugadoresService.crearHabilitacion($scope.habilitacion).then(
                                    function (result) {
                                        if (result != null) {
                                            if (result.success != null && result.success == false) {
                                                notificationService.show('red', 'Ups!', result.message);
                                            }
                                            else {
                                                if ($scope.habilitacion.fechaHabilitacion > $scope.habilitacion.fechaInhabilitacion) {
                                                    notificationService.show('red', 'Ups!', "La fecha de vencimiento debe ser mayor que la de hablilitación");
                                                }
                                                else {
                                                    notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                                                    $scope.instancia.JugadoresHabilitaciones.push(result);
                                                    $scope.instancia.habilitado = true;
                                                    $scope.habilitacion = {};
                                                }
                                            }
                                        }
                                        else {
                                            notificationService.show('red', 'Ups!', 'Ocurrió un error. Por favor vuelva a intentar en unos minutos');
                                        }

                                        $scope.isLoadingHabilitacion = false;
                                    }
                                );
                            }
                            else {
                                if ($scope.habilitacion.categoria == "M") {
                                    notificationService.show('red', 'Ups!', "El Numero de habilitacion debe estar entre 000001 y 16000  ");
                                } else if ($scope.habilitacion.categoria == "I") {
                                    notificationService.show('red', 'Ups!', "El Numero de habilitacion debe estar entre 20001 y 21500  ");
                                } else {
                                    notificationService.show('red', 'Ups!', "El Numero de habilitacion debe ser estar entre 25001 y 25500");
                                }

                                $scope.isLoadingHabilitacion = false;
                            }
                        }
                    );



                    return false;
                };

                $scope.submitCategorias = function () {
                    $scope.isLoadingCategorias = true;

                    $scope.categoria.idJugador = $stateParams.id;
                    jugadoresService.cambiarCategoria($scope.categoria).then(
                        function (result) {
                            if (result != null) {
                                if (result.success != null && result.success == false) {
                                    notificationService.show('red', 'Ups!', result.message);
                                }
                                else {
                                    notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                                    $scope.instancia.JugadoresCategorias.push(result);
                                    $scope.instancia.Categorias = result.Categorias;
                                    $scope.categoria = {};
                                }
                            }
                            else {
                                notificationService.show('red', 'Ups!', 'Ocurrió un error. Por favor vuelva a intentar en unos minutos');
                            }

                            $scope.isLoadingCategorias = false;
                        }
                    );

                    return false;
                };

                // Cada vez que cambia la fecha de habilitacion se modifica la de vencimiento 

                $scope.change = function () {

                    if ((($scope.fechaHabilitacionDesde.getYear() + 1900) == 2021) && ($scope.fechaHabilitacionDesde.getMonth() <= 2)) {
                        $scope.fechaHabilitacionHasta = new Date($scope.fechaHabilitacionDesde.getYear() + 1900, 2, 31);
                        $scope.editFechaHabilitacionHasta = new Date($scope.editFechaHabilitacionHasta.getYear() + 1900, 2, 31)
                    }
                    else {
                        $scope.fechaHabilitacionHasta = new Date($scope.fechaHabilitacionDesde.getYear() + 1901, 2, 31);
                        $scope.editFechaHabilitacionHasta = new Date($scope.editFechaHabilitacionHasta.getYear() + 1901, 2, 31)
                    }

                    //1901 para q sume 1 año
                }

                //Modificar numero de Habilitacion
                $scope.cambiarNumero = function (id, numero, fechadesde, fechahasta) {
                    //modal_habilitacion.showModal();
                    $("#btnHabilitaciones").css('visibility', 'hidden');
                    $("#btnHabilitacionesModificar").css('display', 'block');

                    $('#editNumero').val(numero)


                    $('#editFechaDesde').val($filter('date')(fechadesde, "yyyy-MM-dd"))
                    $('#editfechaHasta').val($filter('date')(fechahasta, "yyyy-MM-dd"))

                    $(".editCambioNumero").css('visibility', 'hidden');
                    $scope.idHabilitacion = id
                }
                $scope.cancelarEdit = function () {
                    $("#btnHabilitaciones").css('visibility', 'visible');
                    $("#btnHabilitacionesModificar").css('display', 'none');
                    $(".editCambioNumero").css('visibility', 'visible');

                }

                $scope.eliminarHabilitacion = function (id) {

                    confirmarEliminar.showModal()
                        .then(function (result) {
                            if (result == 'aceptar') {
                                jugadoresService.eliminarHabilitacion(id).then(
                                    function (result) {
                                        if (result.success == 'true') {
                                            notificationService.show('green', 'Perfecto!', 'La habilitación se elimino correctamente');
                                            jugadoresService.actualizarHabilitaciones($stateParams.id).then(
                                                function (result) {
                                                    $scope.instancia.JugadoresHabilitaciones = result;
                                                }
                                            );
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


                $scope.submitHabilitacionEdit = function () {
                    $scope.isLoadingHabilitacion = true;

                    $scope.habilitacion.idJugador = $stateParams.id;


                    jugadoresService.obtenerEdad($stateParams.id).then(
                        function (result) {
                            $scope.habilitacion.categoria = result;
                            //if (($scope.habilitacion.categoria == "M" && ($scope.habilitacionEditnumero >= 00001 && $scope.habilitacionEditnumero <= 16000)) ||
                            //    ($scope.habilitacion.categoria == "I" && $scope.habilitacionEditnumero >= 20001 && $scope.habilitacionEditnumero <= 21500) ||
                            //    ($scope.habilitacion.categoria == "A" && $scope.habilitacionEditnumero >= 25001 && $scope.habilitacionEditnumero <= 25500)) {

                           
                                jugadoresService.obtenerEstampilla($scope.idHabilitacion).then(
                                    function (result) {
                                        $scope.editHabilitacion = result;
                                        if ($scope.habilitacionEditnumero != null) {
                                            $scope.editHabilitacion.numero = $scope.habilitacionEditnumero;
                                        }
                                        if ($scope.editFechaHabilitacionDesde != null) {
                                            $scope.editHabilitacion.fechaHabilitacion = $scope.editFechaHabilitacionDesde;
                                        }
                                        if ($scope.editFechaHabilitacionHasta != null) {
                                            $scope.editHabilitacion.fechaInhabilitacion = $scope.editFechaHabilitacionHasta;
                                        }
                                        $scope.editHabilitacion.idJugador = $stateParams.id;



                                        jugadoresService.editarHabilitacion($scope.editHabilitacion).then(
                                            function (result) {
                                                if (result != null) {
                                                    if (result.success != null && result.success == false) {
                                                        notificationService.show('red', 'Ups!', result.message);
                                                    }
                                                    else {
                                                        if ($scope.habilitacion.fechaHabilitacion > $scope.habilitacion.fechaInhabilitacion) {
                                                            notificationService.show('red', 'Ups!', "La fecha de vencimiento debe ser mayor que la de hablilitación");
                                                        }
                                                        else {
                                                            notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');


                                                        }
                                                    }
                                                }
                                                else {
                                                    notificationService.show('red', 'Ups!', 'Ocurrió un error. Por favor vuelva a intentar en unos minutos');
                                                }

                                                $scope.isLoadingHabilitacion = false;
                                            }
                                        );
                                        return false;
                                    });

                                jugadoresService.actualizarHabilitaciones($stateParams.id).then(
                                    function (result) {
                                        $scope.instancia.JugadoresHabilitaciones = result;
                                    }
                                );



                            //}
                            //else {
                            //    if ($scope.habilitacion.categoria == "M") {
                            //        notificationService.show('red', 'Ups!', "El Numero de habilitacion debe estar entre 00001 y 16000  ");
                            //    } else if ($scope.habilitacion.categoria == "I") {
                            //        notificationService.show('red', 'Ups!', "El Numero de habilitacion debe estar entre 20001 y 21500  ");
                            //    } else {
                            //        notificationService.show('red', 'Ups!', "El Numero de habilitacion debe ser estar entre 25001 y 25500");
                            //    }

                            //    $scope.isLoadingHabilitacion = false;
                          //  }
                        }
                    );






                    $scope.cancelarEdit();

                }



            }]);

