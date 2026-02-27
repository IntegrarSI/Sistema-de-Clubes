angular.module('site')
    .controller('noticiasCtrl', ['$scope', '$rootScope', 'noticiasService', function ($scope, $rootScope, noticiasService) {



        //se busacan los eventos activos
        $scope.isLoading = true;
        $scope.filter = '';

        $rootScope.loadingLoad = true;
        $scope.init = function () {
            $scope.isLoading = true;
            noticiasService.obtenerTodas($scope.currentPage, $scope.viewby).then(
                function (result) {
                    $scope.isLoading = false;
                    $scope.listaNoticias = result.paginaActual;
                    $scope.totalItems = result.totalItems;
                    console.log($scope.totalItems);

                    for (var i = 0; i < $scope.listaNoticias.length; i++) {
                        if ($scope.listaNoticias[i].fotoPeque == null) {
                            $scope.listaNoticias[i].fotoPeque = "imagen-vacia.png"
                        }
                    }

                    $rootScope.loadingLoad = false;
                }
            );
        };


        $scope.submit = function () {
            $scope.init();
            return false;
        };

        //paginacion
        $scope.viewby = 4;
        $scope.totalItems = 0;
        $scope.currentPage = 1;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5; //Number of pager buttons to show
        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };
        $scope.pageChanged = function () {
            $scope.init();
        };
        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1; //reset to first paghe
        }

        $scope.init();
        ////paginacion

        //$scope.viewby = 4;
        //$scope.totalItems = 0;
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = $scope.viewby;
        //$scope.maxSize = 10; //Number of pager buttons to show



        //$scope.submit = function () {
        //    $scope.init();
        //    return false;
        //};
        //$scope.setPage = function (pageNo) {
        //    $scope.currentPage = pageNo;
        //};
        //$scope.pageChanged = function () {
        //    $scope.init();
        //};
        //$scope.setItemsPerPage = function (num) {
        //    $scope.itemsPerPage = num;
        //    $scope.currentPage = 1; //reset to first paghe
        //}

       

        //$scope.init();
    }]);
