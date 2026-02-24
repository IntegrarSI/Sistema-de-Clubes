angular.module('admin')
    .controller('dashboardCtrl',
    ['$scope', '$rootScope', 'auth', 'pasesService', 'notificationService',
    function ($scope, $rootScope, auth, pasesService, notificationService) {

        $scope.ticket = auth.me();

        $rootScope.loadingLoad = true;
        $scope.buscar = function () {
            pasesService.obtenerListadoPendientes($scope.currentPage).then(
                function (result) {
                    $scope.pasesPendientes = result.paginaActual;
                    $rootScope.loadingLoad = false;
                }
            );
        };

        $scope.submit = function () {
            $scope.buscar();
            return false;
        };

        $scope.buscar();

        $scope.aprobarPase = function (item) {
            item.isLoading = true;
            pasesService.aprobarPase(item.id).then(
                function (result) {
                    if (result.success == 'true') {
                        notificationService.show('green', 'Perfecto!', 'El pase fue aprobado correctamente');
                        item.visible = false;
                    }
                    else
                        notificationService.show('red', 'Ups!', result.message);

                    item.isLoading = false;
                }
            );
        };

        $scope.revisarPase = function (item) {
            item.isLoading = true;
            pasesService.revisarPase(item.id).then(
                function (result) {
                    if (result.success == 'true') {
                        notificationService.show('green', 'Perfecto!', 'El pase fue actualizado correctamente');
                        item.PasesEstados = { codigoInterno: 'revision', descripcion: 'EN REVISIÓN' };
                    }
                    else
                        notificationService.show('red', 'Ups!', result.message);

                    item.isLoading = false;
                }
            );
        };

        $scope.guardarRechazarPase = function (item) {
            item.isLoading = true;
            pasesService.rechazarPase(item.id, item.motivosRechazo).then(
                function (result) {
                    if (result.success == 'true') {
                        notificationService.show('green', 'Perfecto!', 'El pase fue rechazado correctamente');
                        item.visible = false;
                    }
                    else
                        notificationService.show('red', 'Ups!', result.message);

                    item.isLoading = false;
                }
            );
        };

        $scope.rechazarPase = function (item) {
            item.rechazar = true;
            console.log('rechazar');
        };

        $scope.cancelarRechazarPase = function (item) {
            item.rechazar = false;
        };

    }]);
