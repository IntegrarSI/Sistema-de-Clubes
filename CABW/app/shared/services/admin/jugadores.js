angular.module('admin')
    .service('jugadoresService',
        ['$http', '$q',
        function ($http, $q) {

            var _obtenerListado = function (filtro, filtroFederacion, filtroAsociacion, filtroClub, pageIndex) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtenerListado?filtro=' + filtro + '&filtroFederacion=' + filtroFederacion+ '&filtroAsociacion=' + filtroAsociacion+
                    '&filtroClub=' + filtroClub + '&pageIndex=' + pageIndex)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _obtener = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtener?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };


            var _obtenerPorDni = function (Dni) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtener?id=' + Dni)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _modificar = function (instancia) {
                var deffered = $q.defer();
                $http.post('/jugadores/modificar', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _crearHabilitacion = function (instancia) {
                var deffered = $q.defer();
                $http.post('/jugadores/crearHabilitacion', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _eliminarHabilitacion = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/eliminarHabilitacion?id='+ id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _crearPase = function (instancia) {
                var deffered = $q.defer();
                $http.post('/jugadores/crearPase', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _crearPenalizacion = function (instancia) {
                var deffered = $q.defer();
                $http.post('/jugadores/crearPenalizacion', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _cambiarCategoria = function (instancia) {
                var deffered = $q.defer();
                $http.post('/jugadores/cambiarCategoria', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _entregarCarnet = function (instancia) {
                var deffered = $q.defer();
                $http.post('/jugadores/entregarCarnet', instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _solicitarCarnet = function (id,motivo) {
                var deffered = $q.defer();
                $http.post('/jugadores/solicitarCarnet?id=' + id + '&motivo=' + motivo)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _obtenerEstampilla = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtenerEstampilla?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };
         

            var _editarHabilitacion = function (instancia) {
                var deffered = $q.defer();
                $http.post('/jugadores/editarHabilitacion',instancia)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            var _actualizarHabilitaciones = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/actualizarHabilitaciones?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            
            var _eliminar = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/eliminar?id='+id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

           
            var _obtenerEdad = function (id) {
                var deffered = $q.defer();
                $http.post('/jugadores/obtenerEdad?id=' + id)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                        var result = { success: false, message: 'Ocurrió un error en el sistema' };
                        deffered.resolve(result);
                    });

                return deffered.promise;
            };

            return {
                obtenerListado: _obtenerListado,
                obtener: _obtener,
                entregarCarnet: _entregarCarnet,
                solicitarCarnet: _solicitarCarnet,
                crearHabilitacion: _crearHabilitacion,
                eliminarHabilitacion: _eliminarHabilitacion,
                cambiarCategoria: _cambiarCategoria,
                crearPase: _crearPase,
                crearPenalizacion: _crearPenalizacion,
                modificar: _modificar,
                obtenerPorDni: _obtenerPorDni,
                obtenerEstampilla: _obtenerEstampilla,
                editarHabilitacion: _editarHabilitacion,
                actualizarHabilitaciones: _actualizarHabilitaciones,
                eliminar: _eliminar,
                obtenerEdad: _obtenerEdad
               
            }
        }]);