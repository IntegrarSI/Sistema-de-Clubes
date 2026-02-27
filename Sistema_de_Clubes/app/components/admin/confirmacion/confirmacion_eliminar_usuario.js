angular.module('admin').service('confirmarEliminarUsuario', ['$uibModal',
    function ($uibModal) {

        var modalDefaults = {
            backdrop: true,
            keyboard: true,
            modalFade: true,
            templateUrl: '/app/components/admin/confirmacion/confirmacion_eliminar_usuario.html'
        };

        var modalOptions = {
            closeButtonText: 'Cerrar',
            actionButtonText: 'Ok',
            headerText: 'Confirmación para eliminar jugador',
            bodyText: '¿Esta seguro que desea eliminar el jugador?',
            typeMessage: ''
        };

        this.showModal = function (/*type, headerText, bodyText*/) {
            customModalDefaults = {};
            customModalDefaults.backdrop = 'static';

            var modalOptions = {
                closeButtonText: 'Cancelar',
                actionButtonText: 'Aceptar'
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
                        $uibModalInstance.close('aceptar');
                    };
                    $scope.modalOptions.close = function (result) {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            }

            return $uibModal.open(tempModalDefaults).result;
        };



    }]);