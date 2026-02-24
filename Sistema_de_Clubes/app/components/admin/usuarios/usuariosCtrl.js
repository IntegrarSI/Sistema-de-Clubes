angular.module('admin')
    .controller('usuariosCtrl', ['$scope', '$rootScope', 'usuariosService', 'notificationService',
        function ($scope, $rootScope, usuariosService, notificationService) {

        $scope.isLoading = true;
        $scope.filter = '';

        $rootScope.loadingLoad = true;
        $scope.buscar = function () {
            $scope.isLoading = true;
            usuariosService.obtenerListado($scope.filter, $scope.currentPage).then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.grilla = result.paginaActual;
                    $scope.totalItems = result.totalItems;

                    $rootScope.loadingLoad = false;
                }
            );
        };

        $scope.submit = function () {
            $scope.buscar();
            return false;
        };

        $scope.resetPassword = function (item) {
            $scope.isLoading = true;
            usuariosService.resetPassword(item.id).then(
                function (result) {
                    if (result.success == 'true')
                        notificationService.show('green', 'Perfecto!', 'La contraseña ha sido reseteada correctamente. La nueva contraseña es ' + result.message);
                    else
                        notificationService.show('red', 'Ups!', result.message);

                    $scope.isLoading = false;
                }
            );
        };


        $scope.borrarUsuario = function (id) {
            $scope.isLoading = true;
            usuariosService.borrarUsuario(id).then(
                function (result) {
                    if (result.success == 'true')
                        notificationService.show('green', 'Perfecto!', 'El usuario se elimino correctamente');
                    else
                        notificationService.show('red', 'Ups!', result.message);

                    $scope.isLoading = false;
                    $scope.isLoading = true;
                    usuariosService.obtenerListado($scope.filter, $scope.currentPage).then(
                        function (result) {
                            $scope.isLoading = false;
                            $scope.grilla = result.paginaActual;
                            $scope.totalItems = result.totalItems;

                            $rootScope.loadingLoad = false;
                        })
                }
            );
        };

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

        $scope.buscar();

    }]);
