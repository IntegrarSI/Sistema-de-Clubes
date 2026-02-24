angular.module('admin')
    .controller('rolesCtrl', ['$scope', 'rolesService', function ($scope, rolesService) {

        $scope.isLoading = true;
        $scope.filter = '';

        $scope.buscar = function () {
            $scope.isLoading = true;
            rolesService.obtenerListado($scope.filter, $scope.currentPage).then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.grilla = result.paginaActual;
                    $scope.totalItems = result.totalItems;
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
