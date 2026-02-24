angular.module('site').service('buscarCarnet', ['$uibModal','jugadoresService',
    function ($uibModal, jugadoresService) {

        var modalDefaults = {
            backdrop: true,
            keyboard: true,
            modalFade: true,
            templateUrl: '/app/components/site/buscar_carnet/buscar_carnet.html'
        };

        var modalOptions = {
            closeButtonText: 'Cerrar',
            actionButtonText: 'Ok',
            headerText: '',
            bodyText: '',
            typeMessage: ''
        };

        this.showModal = function (/*type, headerText, bodyText*/) {
            customModalDefaults = {};
            customModalDefaults.backdrop = 'static';

            var modalOptions = {
                closeButtonText: 'Cancelar',
                actionButtonText: 'Buscar'
            };

            return this.show(customModalDefaults, modalOptions);
        };

        /*this.showModal = function (customModalDefaults, customModalOptions) {
            if (!customModalDefaults) customModalDefaults = {};
            customModalDefaults.backdrop = 'static';
            return this.show(customModalDefaults, customModalOptions);
        };*/

       
        this.show = function (customModalDefaults, customModalOptions) {
            //Create temp objects to work with since we're in a singleton service
            var tempModalDefaults = {};
            var tempModalOptions = {};

            //Map angular-ui modal custom defaults to modal defaults defined in service
            angular.extend(tempModalDefaults, modalDefaults, customModalDefaults);

            //Map modal.html $scope custom properties to defaults defined in service
            angular.extend(tempModalOptions, modalOptions, customModalOptions);

            if (!tempModalDefaults.controller) {
                tempModalDefaults.controller = function ($scope, $uibModalInstance) {
                    $scope.modalOptions = tempModalOptions;
                    $scope.modalOptions.ok = function (result) {
                        $uibModalInstance.close(result);
                    };
                    $scope.modalOptions.close = function (result) {
                        $uibModalInstance.dismiss('cancel');
                    };
                    $scope.modalOptions.findId = function (result) {
                        jugadoresService.obtenerUrl().then(
                            function (result) {
                                $scope.url = result;
                        });

                        //llamar al servicio que busca el id del jugador
                        $scope.instancia = {};
                        jugadoresService.obtenerPorDni($scope.dni).then(
                            function (result) {
                                $scope.isLoading = false;
                                $scope.instancia = result;
                                var id = $scope.instancia.id;
                                //cambiar para usar la url en el webconfig
                                window.open("Home/jugadores/#/jugadores/carnet/" + id);
                                $rootScope.loadingLoad = false;
                            });
                        

                    };
                }
            }

            return $uibModal.open(tempModalDefaults).result;
        };



    }]);