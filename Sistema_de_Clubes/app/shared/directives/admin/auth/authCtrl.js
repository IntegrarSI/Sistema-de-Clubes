angular.module('admin')
    .controller('authCtrl', ['$scope', '$rootScope', 'auth', function ($scope, $rootScope, auth) {

        $scope.ticket = auth.me();

        $scope.logout = function () {
            auth.logout();
        };

        // se verifica el tamaño de la pantalla para activar que el menú se cierre luego de hacerle clic
        if (window.innerWidth < 768) {
            $rootScope.dataToggle = "collapse";
            $rootScope.isMobile = true;
        } else {
            $rootScope.dataToggle = "";
            $rootScope.isMobile = false;
        }

        // control del menú desplegable
        $scope.reportsOpen = false;
        $scope.securityOpen = false;

        $scope.toggleReports = function ($event) {
            if ($event) {
                $event.preventDefault();
            }

            $scope.reportsOpen = !$scope.reportsOpen;

            if ($scope.reportsOpen) {
                $scope.securityOpen = false;
            }
        };

        $scope.toggleSecurity = function ($event) {
            if ($event) {
                $event.preventDefault();
            }

            $scope.securityOpen = !$scope.securityOpen;

            if ($scope.securityOpen) {
                $scope.reportsOpen = false;
            }
        };

    }]);