angular.module('admin')
    .service('informesService',
        ['$http', '$q',
        function ($http, $q) {

            var _informes = function (filtros, informe) {
                var deffered = $q.defer();
                $http.post('/informes/' + informe, filtros)
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            var _guardar = function () {
                var deffered = $q.defer();
                $http.post('/informes/guardar')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            //var jugadoresHabilitadosExport = function () {
            //    var deffered = $q.defer();
            //    $http.post('/informes/jugadoresHabilitadosExport')
            //        .success(function (data) {
            //            deffered.resolve(data);
            //        }).error(function (data, status, headers, config) {
            //            console.log(data);
            //        });

            //    return deffered.promise;
            //};

            
            var _obtenerNroExportacion = function () {
                var deffered = $q.defer();
                $http.post('/informes/obtenerNroExportacion')
                    .success(function (data) {
                        deffered.resolve(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });

                return deffered.promise;
            };

            
            return {
                guardar:_guardar,
                carnetsNoEntregados: function (filtros) { return _informes(filtros, 'carnetsNoEntregados') },
                jugadoresHabilitados: function (filtros) { return _informes(filtros, 'jugadoresHabilitados') },
                totalJugadores: function (filtros) { return _informes(filtros, 'totalJugadores') },
                pases: function (filtros) { return _informes(filtros, 'pases') },
                cambiosDeCategoria: function (filtros) { return _informes(filtros, 'cambiosDeCategoria') },
                obtenerNroExportacion: _obtenerNroExportacion
                //jugadoresHabilitadosExport: _jugadoresHabilitadosExport
                
            }
        }]);