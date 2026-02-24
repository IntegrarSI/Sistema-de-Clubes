angular.module('site')
    .controller('historiaCtrl', ['$scope', '$rootScope', 'historiaModal', 'campeonesService', function ($scope, $rootScope, historiaModal, campeonesService) {

       
        $scope.isLoading = true;
        $scope.filter = '';





        $rootScope.loadingLoad = true;
        $scope.buscar = function (pTorneo) {
            $scope.isLoading = true;
            campeonesService.obtenerListado($scope.currentPage, $scope.viewby, pTorneo).then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.grilla = result.paginaActual;
                    $scope.totalItems = result.totalItems;
                    $rootScope.loadingLoad = false;
                }
            );
        };


        $scope.ultimos = function () {
            $scope.isLoading = true;
            campeonesService.obtenerUltimos().then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.ultimosCampeones = result;
                    $rootScope.loadingLoad = false;
                }
            );
        };

        //paginacion
        $scope.viewby = 10;
        $scope.totalItems = 0;
        $scope.currentPage = 2;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5; //Number of pager buttons to show
        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };
        $scope.pageChanged = function (pTorneo) {
            $scope.buscar(pTorneo);
        };
        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1; //reset to first paghe
        }

        $scope.ultimos();
    }]);
