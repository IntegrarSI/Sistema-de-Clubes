angular.module('admin')
    .controller('asociacionesCtrl', ['$scope', '$rootScope', 'asociacionesService', 'federacionesService',
        function ($scope, $rootScope, asociacionesService, federacionesService) {

        $scope.isLoading = true;
        $scope.filter = '';
        $scope.federacion = { id: 0, descripcion: 'Todas' };

        //lista de federaciones
        federacionesService.obtenerLista().then(
                function (result) {
                    $scope.federaciones = result;
                    if (result.length > 1)
                        $scope.federaciones.push($scope.federacion);
                }
            );

        $rootScope.loadingLoad = true;

        $scope.buscar = function () {
            $scope.isLoading = true;
            asociacionesService.obtenerListado($scope.filter, $scope.federacion.id, $scope.currentPage).then(
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
