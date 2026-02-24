angular.module('site')
    .controller('federacionesCtrl', ['$scope', 'db', function ($scope, db) {
        
        $scope.isLoadingFederaciones = true;
        $scope.federacion = null;
        $scope.asociacion = null;
        $scope.club = null;
        
        $scope.init = function () {
            $scope.isLoadingFederaciones = true;
            db
                .federaciones()
                .success(function (result) {
                    $scope.isLoadingFederaciones = false;
                    $scope.federaciones = result;
                });
        }

        $scope.init();

        $scope.seleccionarFederacion = function (item) {
            $scope.asociaciones = null;
            $scope.asociacion = null;
            $scope.clubes = null;
            $scope.club = null;
            $scope.federacion = item;
            $scope.obtenerAsociaciones(item.id);
        };

        //asociaciones
        $scope.obtenerAsociaciones = function (id) {
            $scope.isLoadingAsociaciones = true;
            db
                .asociaciones(id)
                .success(function (result) {
                    $scope.isLoadingAsociaciones = false;
                    $scope.asociaciones = result;
                });
        }

        $scope.seleccionarAsociacion = function (item) {
            $scope.clubes = null;
            $scope.club = null;
            $scope.asociacion = item;
            $scope.obtenerClubes(item.id);
        };

        //clubes
        $scope.obtenerClubes = function (id) {
            $scope.isLoadingClubes = true;
            db
                .clubes(id)
                .success(function (result) {
                    $scope.isLoadingClubes = false;
                    $scope.clubes = result;
                });
        }

        $scope.seleccionarClub = function (item) {
            $scope.club = item;
        };

    }]);
