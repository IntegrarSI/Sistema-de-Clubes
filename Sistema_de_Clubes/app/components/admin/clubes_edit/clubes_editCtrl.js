angular.module('admin')
    .controller('clubes_editCtrl',
        ['$scope', '$rootScope', '$state', 'focus', '$stateParams', 'notificationService', 'clubesService', 'federacionesService', 'asociacionesService', 'localidadesService', 'FileUploader'
        , function ($scope, $rootScope, $state, focus, $stateParams, notificationService, clubesService, federacionesService, asociacionesService, localidadesService, FileUploader) {
       
            $scope.esAlta = false;
            if ($stateParams.id == '') {
                $scope.title = 'Nuevo Club';
                $scope.esAlta = true;
            }
            else
                $scope.title = 'Modificación de clubes';


            /* Uploader */
            var uploader = $scope.uploader = new FileUploader({
                url: '/clubes/UploadFile',
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
                    $scope.instancia.logo = response.fileName;

                $scope.isLoading = false;
            };
            uploader.onErrorItem = function (fileItem, response, status, headers) {
                $scope.isLoading = false;
            };
            
            $scope.init = function () {

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
                        }
                    );
                }

                $scope.instancia = {};
                $scope.localidad = {};

                $scope.isLoading = false;
                focus('descripcion');
                
                if ($stateParams.id != '') {

                    $rootScope.loadingLoad = true;

                    //es una modificacion
                    $scope.isLoading = true;
                    clubesService.obtener($stateParams.id).then(
                        function (result) {
                            $scope.instancia = result;
                            $scope.instancia.idFederacion = $scope.instancia.Asociaciones.idFederacion;

                            if ($scope.instancia.Localidades != null)
                                $scope.instancia.localidad = $scope.instancia.Localidades.descripcion + ' - ' + $scope.instancia.Localidades.Provincias.descripcion;

                            $scope.isLoading = false;
                            $rootScope.loadingLoad = false;
                        });
                }
                else {

                    $scope.instancia.activo = true;

                }
            }

            $scope.$watch('instancia.idFederacion', function () {
                //se cargan las asociaciones cuándo cambia la federacion
                if ($scope.instancia.idFederacion) {
                    asociacionesService.obtenerLista($scope.instancia.idFederacion).then(
                        function (result) {
                            $scope.asociaciones = result;
                        }
                    );
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

            $scope.submit = function () {
                $scope.isLoading = true;
                //$scope.instancia.logo = uploader.queue[0].file;
                clubesService.modificar($scope.instancia).then(
                    function (result) {
                        if (result.success == 'true') {
                            notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                            $state.go('clubes');
                        }
                        else
                            notificationService.show('red', 'Ups!', result.message);

                        $scope.isLoading = false;
                    }
                );

                return false;
            };


    }]);

